using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Mapping;
using GameCore.Misc;
using OpenTK.Graphics.OpenGL;
using Point = GameCore.Misc.Point;

namespace Shader
{
    struct ShadowCaster
    {
		public float Opacity;
        public LiveMapCell LiveMapCell;

	    public override string ToString()
	    {
			return Opacity >0? "X" : "";
	    }
    }

	struct Light
	{
		public PointF Point;
		public ILightSource LightSource;

		public override string ToString()
		{
			return LightSource==null?"":LightSource.ToString();
		}
	}

    public class LosManagerEx
    {
        private const int MAP_SIZE = Constants.MAP_BLOCK_SIZE * 3;
        private static readonly Point[] m_allBlockPoints;
	    private static readonly EdgeEx[] m_allEdges = new EdgeEx[MAP_SIZE * MAP_SIZE * 4];
		private static readonly Light[] m_lights = new Light[100];

		static LosManagerEx()
		{
			m_allBlockPoints = Point.Zero.GetAllBlockPoints().ToArray();
		}

		private readonly ShadowCaster[,] m_shadowCasters = new ShadowCaster[MAP_SIZE, MAP_SIZE];
		private int m_edgesCount;
		private int m_lightsCount;
		private FboWrapper m_fbo;
		private FboWrapper m_fboBlit;
		
		public LosManagerEx()
	    {
			m_fbo = new FboWrapper(true);
			m_fboBlit = new FboWrapper(false);
	    }

	    public void Recalc(LiveMap _liveMap)
        {
			m_lightsCount = 0;
			m_edgesCount = 0;
			using (new Profiler("LosManagerEx.Recalc"))
            {
                #region подготовка карты

                var liveBlocks = _liveMap.GetLightedLiveBlocks();
                var point = new Point(1, 1);

                for (var i = 0; i < 9; i++)
                {
                    var dlt = (liveBlocks[i, 0] + point)*Constants.MAP_BLOCK_SIZE;
                    var livBlockXY = liveBlocks[i, 1];
                    for (var index = 0; index < m_allBlockPoints.Length; index++)
                    {
                        var blockPoint = m_allBlockPoints[index];
						var liveCellXY = livBlockXY * Constants.MAP_BLOCK_SIZE + blockPoint;
                        var liveMapCell = _liveMap.Cells[liveCellXY.X, liveCellXY.Y];

                        var opacity = liveMapCell.CalcOpacity();

                        if (opacity < float.Epsilon)
                        {
                            m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].Opacity = 0;
                        }
                        else
                        {
                            m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].Opacity = opacity;
                            m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].LiveMapCell = liveMapCell;
                        }

	                    #region источники света от существ

	                    var creature = liveMapCell.Creature;
	                    if (creature == null || creature.Light == null) continue;

	                    var tempPoint = dlt + blockPoint;
	                    m_lights[m_lightsCount].Point = new PointF(tempPoint.X,tempPoint.Y);
	                    m_lights[m_lightsCount++].LightSource = creature.Light;

	                    #endregion
                    }

	                #region остальные источники света

	                foreach (var info in _liveMap.Blocks[livBlockXY.X, livBlockXY.Y].MapBlock.LightSources)
	                {
						var tempPoint = dlt + info.Point;
		                m_lights[m_lightsCount].Point = new PointF(tempPoint.X,tempPoint.Y);
		                m_lights[m_lightsCount++].LightSource = info.Source;
	                }

	                #endregion
                }

                #endregion

                m_edgesCount = 0;

                for (var i = 0; i < MAP_SIZE; ++i)
                {
                    for (var j = 0; j < MAP_SIZE; ++j)
                    {
						var lmc = m_shadowCasters[i, j].LiveMapCell;
	                    if (lmc == null) continue;

	                    var opacity = m_shadowCasters[i, j].Opacity;
	                    var rect = new RectangleF(i, j, 1, 1);
	                    if (j > 0 && m_shadowCasters[i, j - 1].LiveMapCell == null)
	                    {
		                    m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top)) { Opacity = opacity, TransparentColor = lmc.TransparentColor };
	                    }
	                    if (i < (MAP_SIZE - 1) && m_shadowCasters[i + 1, j].LiveMapCell == null)
	                    {
		                    m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom)) { Opacity = opacity, TransparentColor = lmc.TransparentColor };
	                    }
	                    if (j < (MAP_SIZE - 1) && m_shadowCasters[i, j + 1].LiveMapCell == null)
	                    {
		                    m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom)) { Opacity = opacity, TransparentColor = lmc.TransparentColor };
	                    }
	                    if (i > 0 && m_shadowCasters[i - 1, j].LiveMapCell == null)
	                    {
		                    m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top)) { Opacity = opacity, TransparentColor = lmc.TransparentColor };
	                    }
                    }
                }

				var dPoint = _liveMap.GetDPoint();
                var viewportSize = _liveMap.VieportSize;

	            while (m_fboBlit.CountOfBuffers<m_lightsCount)
	            {
		            m_fboBlit.AddTextureBuffer();
	            }

	            for (var i = 0; i < m_lightsCount; i++)
	            {
		            DrawShadows(m_lights[i]);
	            }
            }
        }

		private void DrawShadows(Light _light)
		{
			GL.ClearColor(0f, 0f, 0f, 0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.Begin(BeginMode.Polygon);
			{

				GL.Color3(1f, 0.7f, 0.7f);

				GL.Vertex2(_light.Point.X, _light.Point.Y);
				GL.Color3(0f, 0f, 0f);
				const float step = (float)Math.PI / 10f;
				for (float f = 0; f < Math.PI * 2 + step; f += step)
				{
					var x = Math.Sin(f) * _light.LightSource.Radius + _light.Point.X;
					var y = Math.Cos(f) * _light.LightSource.Radius + _light.Point.Y;
					GL.Vertex2(x, y);
				}
				GL.Color3(1f, 0.7f, 1f);
				GL.Vertex2(_light.Point.X, _light.Point.Y);
			}
			GL.End();

			#region Собираем все грани лицевые для источника освещения и попадающие в круг света


			var edges = new EdgeEx[MAP_SIZE * MAP_SIZE * 4];
			var edgesCount = 0;
			for (var i = 0; i < m_edgesCount; i++)
			{
				var edge = m_allEdges[i];
				if (!(EdgeEx.Distant(edge.P1, _light.Point) < _light.LightSource.Radius*2) || !(edge.Orient(_light.Point) >= 0)) continue;

				edges[edgesCount] = edge;
				edges[edgesCount].Distance = Edge.Distant(edge.P1, _light.Point);
				edgesCount++;
			}

			Array.Sort(edges,0,edgesCount, new DistanceComparer());

			#endregion

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Begin(BeginMode.Quads);
			{
				GL.Color3(0f, 0f, 0f);

				for (var i = 0; i < edgesCount; i++)
				{
					if (!edges[i].Valid) continue;
					var pnt = new[] { edges[i].P2, edges[i].P1, GetFarPnt(_light.Point, edges[i].P1), GetFarPnt(_light.Point, edges[i].P2) };
					{
						GL.Vertex2(pnt[0].X, pnt[0].Y);
						GL.Vertex2(pnt[1].X, pnt[1].Y);
						GL.Vertex2(pnt[2].X, pnt[2].Y);
						GL.Vertex2(pnt[3].X, pnt[3].Y);
					}

					#region Отбрасываем все грани вошедшие внутрь теневой трапеции

					var e1 = new EdgeEx(pnt[2], pnt[0]);
					var e2 = new EdgeEx(pnt[0], pnt[3]);

					for (var j = i+1; j < edgesCount; j++)
					{
						if (!edges[j].Valid)
						{
							continue;
						}
						if (e1.Orient(edges[j].P1) < 0 && e2.Orient(edges[j].P1) < 0)
						{
							edges[j].Valid = false;
						}
					}

					#endregion
				}
			}
			GL.End();
			GL.Disable(EnableCap.Blend);
		}

		public class DistanceComparer : IComparer<EdgeEx>
		{
			public int Compare(EdgeEx _x, EdgeEx _y)
			{
				return _y.Distance.CompareTo(_x.Distance);
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
			//var md = LIGHTRADIUS / v.Length;
			return v * md;
		}
    }

	public class EdgeEx
	{
		public PointF P1;
		public PointF P2;

		public bool Valid;
		public float Distance;
		public float Opacity;

		public EdgeEx(PointF _p1, PointF _p2) 
		{
			Valid = true;
			P1 = _p1;
			P2 = _p2;
			Distance = 0;
			Opacity = 0;
			TransparentColor = FColor.Empty;
		}

		public Vector Vector
		{
			get { return new Vector(P1, P2); }
		}

		public FColor TransparentColor;

		public override string ToString()
		{
			return string.Format("{2}[{0};{1}]", P1, P2, Valid ? "+" : "-");
		}

		public static float Distant(PointF p1, PointF p2)
		{
			var d = new PointF(p1.X - p2.X, p1.Y - p2.Y);
			return (float)Math.Sqrt(d.X * d.X + d.Y * d.Y);
		}

		/// <summary>
		///     Проверка ориентации отрезка относительно точки
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public float Orient(PointF p)
		{
			return (P1.X - p.X) * (P2.Y - p.Y) - (P1.Y - p.Y) * (P2.X - p.X);
		}
	}

}
