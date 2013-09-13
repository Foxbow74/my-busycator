using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Mapping;
using GameCore.Misc;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Point = GameCore.Misc.Point;

namespace Shader
{
    public class LosManagerEx
    {
        #region shaders

        private const string LIGHT_SHADER_SOURCE = @"
uniform vec2 _position;
uniform sampler2D _light;
void main(void)
{
    vec2 xy = gl_FragCoord.xy;
    float d = distance(xy,_position)+0.1;
    vec3 c = texture2D( _light, gl_TexCoord[0].st).rgb * min(1.0,@MAX_VISIBLE_RADIUS0.0/d/d);
    gl_FragColor = vec4(c[0],c[1],c[2],1.0);
}";

        private const string SIGHT_SHADER_SOURCE = @"
uniform vec2 _position;
uniform sampler2D _sight;
void main(void)
{
    vec2 xy = gl_FragCoord.xy;
    float d = distance(xy,_position);
    vec3 c = texture2D( _sight, gl_TexCoord[0].st).rgb * (1.0- d/ @MAX_VISIBLE_RADIUS.0);
    gl_FragColor = vec4(c[0],c[1],c[2],1.0);
}";

        #endregion

        private const int MAX_VISIBLE_RADIUS = 16;
        public const int MAP_SIZE = Constants.MAP_BLOCK_SIZE * 3;
        private static readonly Point[] m_allBlockPoints;
	    private static readonly EdgeEx[] m_allEdges = new EdgeEx[MAP_SIZE * MAP_SIZE * 4];
        private static readonly Light[] m_lights = new Light[100];
        private static readonly FColor[,] m_lightMap = new FColor[MAP_SIZE, MAP_SIZE];
        private static readonly ShaderWrapper m_lightShader;
        private static readonly ShaderWrapper m_sightShader;


		static LosManagerEx()
		{
			m_allBlockPoints = Point.Zero.GetAllBlockPoints().ToArray();

            m_lightShader = new ShaderWrapper(LIGHT_SHADER_SOURCE.Replace("@MAX_VISIBLE_RADIUS",MAX_VISIBLE_RADIUS.ToString()));
            m_sightShader = new ShaderWrapper(SIGHT_SHADER_SOURCE.Replace("@MAX_VISIBLE_RADIUS", MAX_VISIBLE_RADIUS.ToString()));
		}

        private readonly ShadowCaster[,] m_shadowCasters = new ShadowCaster[MAP_SIZE, MAP_SIZE];
		private int m_edgesCount;
		private int m_lightsCount;
		private readonly FboWrapper m_fbo;
		private readonly FboWrapper m_fboBlit;

        public LosManagerEx()
	    {
			m_fbo = new FboWrapper(true);
			m_fboBlit = new FboWrapper(false);
	    }

	    public void Recalc(LiveMap _liveMap)
	    {

		    m_lightsCount = 0;
		    m_edgesCount = 0;

		    var avatarXY = BaseMapBlock.GetInBlockCoords(World.TheWorld.Avatar.GeoInfo.LiveCoords) + new Point(32, 32);
		    m_lights[m_lightsCount++] = new Light
		                                {
			                                LightSource = new AvatarSight(),
			                                Point = new PointF(avatarXY.X, avatarXY.Y),
			                                LiveMapCell = World.TheWorld.Avatar[0, 0]
		                                };

#if DEBUG
		    using (new Profiler("LosManagerEx.Recalc"))
#endif
            {
			    ПодготовкаКарты(_liveMap, avatarXY);
                СформироватьМассивГраней();
                while (m_fboBlit.CountOfBuffers < (m_lightsCount + 2))
			    {
				    m_fboBlit.AddTextureBuffer();
                }
                Отрисовка(avatarXY);
                ОбновитьСостояниеОсвещенностиКарты(_liveMap);
		    }

		    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		    GL.Enable(EnableCap.Texture2D);
	    }

        private void ОбновитьСостояниеОсвещенностиКарты(LiveMap _liveMap)
        {
#if DEBUG
            using (new Profiler())
#endif
            {
                using (new FboWrapper.DrawHelper(m_fboBlit))
                {
                    GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
                    GL.ReadPixels(0, 0, MAP_SIZE, MAP_SIZE, PixelFormat.Rgba, PixelType.Float, m_lightMap);

                    //FboWrapper.DrawHelper.Screenshot("e:\\1\\scr.png");

                    var dPoint = _liveMap.GetData(); //.GetDPoint();
                    var viewportSize = _liveMap.VieportSize;

                    var layer = World.TheWorld.Avatar.GeoInfo.Layer;
                    var fogLightness = layer.FogLightness;
                    for (var i = 0; i < MAP_SIZE; i++)
                    {
                        for (var j = 0; j < MAP_SIZE; j++)
                        {
                            var lc = m_shadowCasters[j, i].LiveMapCell;
                            lc.Lighted = m_lightMap[i, j];
                            lc.UpdateVisibility(fogLightness);
                        }
                    }
                }
            }
        }

        private void СформироватьМассивГраней()
        {
#if DEBUG
            using (new Profiler())
#endif
            {
                m_edgesCount = 0;

                var hs = new Dictionary<Int32, int>();

                var doubles = 0;

                for (var i = 1; i < MAP_SIZE - 1; ++i)
                {
                    for (var j = 1; j < MAP_SIZE - 1; ++j)
                    {
                        var lmc = m_shadowCasters[i, j].LiveMapCell;
                        lmc.FinalLighted = FColor.Empty;
                        if (lmc == null) continue;

                        var opacity = m_shadowCasters[i, j].Opacity;

                        if (opacity > 0)
                        {
                            if (m_shadowCasters[i, j - 1].Opacity != opacity
                                || m_shadowCasters[i - 1, j - 1].Opacity != opacity
                                || m_shadowCasters[i + 1, j - 1].Opacity != opacity
                                || m_shadowCasters[i, j + 1].Opacity != opacity
                                || m_shadowCasters[i - 1, j + 1].Opacity != opacity
                                || m_shadowCasters[i + 1, j + 1].Opacity != opacity
                                || m_shadowCasters[i, j].Opacity != opacity
                                || m_shadowCasters[i - 1, j].Opacity != opacity
                                || m_shadowCasters[i + 1, j].Opacity != opacity)
                            {
                                var rect = new RectangleF(i, j, 1, 1);

                                Action<PointF, PointF> add = (_a, _b) =>
                                {
                                    var edge = new EdgeEx(_a, _b);
                                    int doubleIndex;
                                    if (hs.TryGetValue(edge.GetHashCode(), out doubleIndex) &&
                                        m_allEdges[doubleIndex].Opacity == opacity)
                                    {
                                        hs.Remove(edge.GetHashCode());
                                        m_edgesCount--;
                                        doubles++;
                                        hs[m_allEdges[m_edgesCount].GetHashCode()] = doubleIndex;
                                        m_allEdges[doubleIndex] = m_allEdges[m_edgesCount];
                                        return;
                                    }
                                    hs[edge.GetHashCode()] = m_edgesCount;
                                    m_allEdges[m_edgesCount++] = new EdgeEx(_a, _b)
                                    {
                                        Opacity = opacity,
                                        LiveMapCell = lmc,
                                        CellCenter = new PointF(i, j)
                                    };
                                };

                                add(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top));
                                add(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom));
                                add(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom));
                                add(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top));
                            }
                        }
                    }
                }
            }
        }

        private void ПодготовкаКарты(LiveMap _liveMap, Point _avatarXY)
        {
#if DEBUG
            using (new Profiler())
#endif
            {
                var liveBlocks = _liveMap.GetLightedLiveBlocks();
                var point = new Point(1, 1);

                for (var i = 0; i < 9; i++)
                {
                    var dlt = (liveBlocks[i, 0] + point)*Constants.MAP_BLOCK_SIZE;
                    var livBlockXY = liveBlocks[i, 1];
                    for (var index = 0; index < m_allBlockPoints.Length; index++)
                    {
                        var blockPoint = m_allBlockPoints[index];
                        var liveCellXY = livBlockXY*Constants.MAP_BLOCK_SIZE + blockPoint;
                        var liveMapCell = _liveMap.Cells[liveCellXY.X, liveCellXY.Y];

                        m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].LiveMapCell = liveMapCell;
                        m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].Opacity = liveMapCell.CalcOpacity();

                        #region источники света от существ

                        var creature = liveMapCell.Creature;
                        if (creature == null || creature.Light == null || _avatarXY.GetDistTill(liveCellXY)>(MAX_VISIBLE_RADIUS*2)) continue;

                        var tempPoint = dlt + blockPoint;
                        m_lights[m_lightsCount].Point = new PointF(tempPoint.X, tempPoint.Y);
                        m_lights[m_lightsCount].LiveMapCell = liveMapCell;
                        m_lights[m_lightsCount++].LightSource = creature.Light;

                        #endregion
                    }

                    #region остальные источники света

                    foreach (var info in _liveMap.Blocks[livBlockXY.X, livBlockXY.Y].MapBlock.LightSources)
                    {
                        var tempPoint = dlt + info.Point;
                        if (_avatarXY.GetDistTill(tempPoint) > (MAX_VISIBLE_RADIUS * 2)) continue;
                        m_lights[m_lightsCount].Point = new PointF(tempPoint.X, tempPoint.Y);
                        m_lights[m_lightsCount++].LightSource = info.Source;
                    }

                    #endregion
                }
            }
        }

        private void Отрисовка(Point _center)
        {
#if DEBUG
            using (new Profiler())
#endif
            {
                using (new Profiler("DrawShadows(each)"))
                for (var i = 0; i < m_lightsCount; i++)
                {
                    using (new FboWrapper.DrawHelper(m_fbo))
                    {
                        DrawShadows(m_lights[i], new PointF(_center.X, _center.Y));
                        m_fbo.BlitTo(m_fboBlit, i);
                    }
                }
                m_fbo.BlitTo(m_fboBlit, m_lightsCount + 1);

                if (false)
                {
                    for (var a = 0; a < m_lightsCount + 1; ++a)
                    {
                        using (new FboWrapper.DrawHelper(m_fboBlit))
                        {
                            GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + a);
                            FboWrapper.DrawHelper.Screenshot("e:\\1\\a" + a + ".png");
                        }
                    }
                }

                using (new FboWrapper.DrawHelper(m_fbo))
                {
                    var ambient = World.TheWorld.Avatar.GeoInfo.Layer.Ambient;
                    GL.ClearColor(ambient.R, ambient.G, ambient.B, 1f);
                    GL.Clear(ClearBufferMask.ColorBufferBit);
                    GL.Color3(1f, 1f, 1f);
                    GL.Enable(EnableCap.Texture2D);

                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcColor);

                    const float sz = MAP_SIZE;
                    const float to = ((float) MAP_SIZE)/FboWrapper.SIZE;

                    using (new Profiler("DrawShadows.lightShader(each)"))
                    using (var sh = new ShaderWrapper.DrawHelper(m_lightShader))
                    {
                        for (var a = 1; a < m_lightsCount; ++a)
                        {
                            sh.BindTexture(m_fboBlit[a], TextureUnit.Texture0, "_light");
                            sh.BindVec2(m_lights[a].Point, "_position");
                            GL.Begin(BeginMode.Quads);
                            {
                                GL.TexCoord2(0f, 0f);
                                GL.Vertex2(0f, 0f);

                                GL.TexCoord2(to, 0f);
                                GL.Vertex2(sz, 0f);

                                GL.TexCoord2(to, to);
                                GL.Vertex2(sz, sz);

                                GL.TexCoord2(0f, to);
                                GL.Vertex2(0f, sz);
                            }
                            GL.End();
                        }
                    }

                    using (new Profiler("DrawShadows.sightShader"))
                    using (var sh = new ShaderWrapper.DrawHelper(m_sightShader))
                    {
                        sh.BindTexture(m_fboBlit[0], TextureUnit.Texture0, "_sight");
                        sh.BindVec2(m_lights[0].Point, "_position");
                        GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.OneMinusSrcAlpha);
                        GL.Begin(BeginMode.Quads);
                        {
                            GL.TexCoord2(0f, 0f);
                            GL.Vertex2(0f, 0f);

                            GL.TexCoord2(to, 0f);
                            GL.Vertex2(sz, 0f);

                            GL.TexCoord2(to, to);
                            GL.Vertex2(sz, sz);

                            GL.TexCoord2(0f, to);
                            GL.Vertex2(0f, sz);
                        }
                        GL.End();
                    }

                    m_fbo.BlitTo(m_fboBlit, 1);
                }
            }
        }

        private void DrawShadows(Light _light, PointF _avatarPoint)
		{
#if DEBUG
            using (new Profiler())
#endif
            {
                GL.Enable(EnableCap.Multisample);

                var lp = new PointF(_light.Point.X + 0.5f, _light.Point.Y + 0.5f);

                GL.Disable(EnableCap.Blend);
                GL.ClearColor(_light.LightSource.Color.R, _light.LightSource.Color.G, _light.LightSource.Color.B, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.Enable(EnableCap.Blend);

                #region Собираем все грани лицевые для источника освещения и попадающие в круг света

                var apnts = new HashSet<PointF>();

                var edgesCount = 0;
                var edges = new EdgeEx[m_edgesCount];

                for (var i = 0; i < m_edgesCount; i++)
                {
                    if (_light.LiveMapCell != null && m_allEdges[i].LiveMapCell == _light.LiveMapCell) continue;
                    if (EdgeEx.Distant(m_allEdges[i].P1, lp) >= _light.LightSource.Radius) continue;
                    var orient = m_allEdges[i].Orient(lp);
                    if (orient <= 0)
                    {
                        continue;
                    }

                    if (m_allEdges[i].Orient(_avatarPoint) > 0 && !apnts.Contains(m_allEdges[i].CellCenter))
                    {
                        apnts.Add(m_allEdges[i].CellCenter);
                    }

                    edges[edgesCount] = m_allEdges[i];
                    edges[edgesCount].Distance = Edge.Distant(m_allEdges[i].CellCenter, lp);
                    edges[edgesCount].Valid = true;
                    edgesCount++;
                }

                Array.Sort(edges, 0, edgesCount, new DistanceComparer());

                #endregion

                GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.OneMinusSrcAlpha);

                var pnts = new HashSet<PointF>();

                GL.Begin(BeginMode.Quads);
                {
                    for (var i = 0; i < edgesCount; i++)
                    {
                        if (!edges[i].Valid) continue;

                        if (!pnts.Contains(edges[i].CellCenter))
                        {
                            pnts.Add(edges[i].CellCenter);
                        }

                        var color = edges[i].LiveMapCell.GetTransparentColor() * _light.LightSource.Color * (1f - edges[i].Opacity);

                        GL.Color4(color.R, color.G, color.B, 1f);

                        var pnt = new[]
                        {
                            edges[i].P2,
                            edges[i].P1,
                            GetFarPnt(lp, edges[i].P1),
                            GetFarPnt(lp, edges[i].P2)
                        };

                        GL.Vertex2(pnt[0].X, pnt[0].Y);
                        GL.Vertex2(pnt[1].X, pnt[1].Y);
                        GL.Vertex2(pnt[2].X, pnt[2].Y);
                        GL.Vertex2(pnt[3].X, pnt[3].Y);

                        #region Отбрасываем все грани вошедшие внутрь теневой трапеции

                        if (edges[i].Opacity > 0.9f)
                        {
                            var e1 = new EdgeEx(pnt[0], pnt[3]);
                            var e2 = new EdgeEx(pnt[2], pnt[1]);

                            for (var j = i + 1; j < edgesCount; j++)
                            {
                                if (!edges[j].Valid)
                                {
                                    continue;
                                }

                                if (!(e1.Orient(edges[j].P2) < -float.Epsilon) ||
                                    !(e2.Orient(edges[j].P1) < -float.Epsilon)) continue;
                                if (!(e1.Orient(edges[j].P1) < -float.Epsilon) ||
                                    !(e2.Orient(edges[j].P2) < -float.Epsilon)) continue;

                                edges[j].Valid = false;
                            }
                        }

                        #endregion
                    }
                }
                GL.End();

                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.Multisample);

                if (_light.LightSource is AvatarSight)
                {
                    GL.Color4(1f, 1f, 1f, 1f);
                    foreach (var pnt in pnts)
                    {
                        if (apnts.Contains(pnt))
                        {
                            GL.Vertex2(pnt.X, pnt.Y);
                        }
                    }
                }
                else
                {
                    GL.Begin(BeginMode.Points);
                    {
                        foreach (var pnt in pnts)
                        {
                            if (apnts.Contains(pnt) && pnt != _avatarPoint)
                            {
                                GL.Color4(1f, 1f, 1f, 1f);
                            }
                            else
                            {
                                GL.Color4(0f, 0f, 0f, 1f);
                            }
                            GL.Vertex2(pnt.X, pnt.Y);
                        }
                    }
                    GL.End();
                }
            }
		}

		public class DistanceComparer : IComparer<EdgeEx>
		{
			public int Compare(EdgeEx _x, EdgeEx _y)
			{
				return _x.Distance.CompareTo(_y.Distance);
			}
		}

		/// <summary>
		/// Получить точку на продолжении луча от источника света до точки
		/// </summary>
		/// <param name="_from"></param>
		/// <param name="_to"></param>
		/// <returns></returns>
		private static PointF GetFarPnt(PointF _from, PointF _to)
		{
			var v = new Vector(_from, _to);
            var md = FboWrapper.SIZE * FboWrapper.SIZE / v.Length;
			return v * md;
		}
    }
}
