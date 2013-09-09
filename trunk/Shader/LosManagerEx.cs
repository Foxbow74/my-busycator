using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using GameCore;
using GameCore.Mapping;
using GameCore.Misc;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
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
		public LiveMapCell LiveMapCell;

		public override string ToString()
		{
			return LightSource==null?"":LightSource.ToString();
		}
	}

	class AvatarSight : ILightSource
	{
		public int Radius { get { return LosManagerEx.MAP_SIZE/2; } }

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			throw new NotImplementedException();
		}

		public FColor Color { get { return FColor.White; } }
	}

    public class LosManagerEx
    {
        public const int MAP_SIZE = Constants.MAP_BLOCK_SIZE * 3;
        private static readonly Point[] m_allBlockPoints;
	    private static readonly EdgeEx[] m_allEdges = new EdgeEx[MAP_SIZE * MAP_SIZE * 4];
        private static readonly Light[] m_lights = new Light[100];
        private static readonly FColor[,] m_lightMap = new FColor[MAP_SIZE, MAP_SIZE];

		static LosManagerEx()
		{
			m_allBlockPoints = Point.Zero.GetAllBlockPoints().ToArray();
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
            //GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.Texture2D);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			m_lightsCount = 0;
			m_edgesCount = 0;

			var center = BaseMapBlock.GetInBlockCoords(World.TheWorld.Avatar.GeoInfo.LiveCoords) + new Point(32, 32);
			m_lights[m_lightsCount++] = new Light { LightSource = new AvatarSight(), Point = new PointF(center.X, center.Y), LiveMapCell = World.TheWorld.Avatar[0,0] };

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

                        //var opacity = liveMapCell.CalcOpacity();

                        m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].Opacity = liveMapCell.CalcOpacity();
                        m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].LiveMapCell = liveMapCell;

	                    #region источники света от существ

	                    var creature = liveMapCell.Creature;
	                    if (creature == null || creature.Light == null) continue;

	                    var tempPoint = dlt + blockPoint;
	                    m_lights[m_lightsCount].Point = new PointF(tempPoint.X,tempPoint.Y);
						m_lights[m_lightsCount].LiveMapCell = liveMapCell;
	                    m_lights[m_lightsCount++].LightSource = creature.Light;

	                    #endregion
                    }

	                #region остальные источники света

	                foreach (var info in _liveMap.Blocks[livBlockXY.X, livBlockXY.Y].MapBlock.LightSources)
	                {
						var tempPoint = dlt + info.Point;
		                m_lights[m_lightsCount].Point = new PointF(tempPoint.X,tempPoint.Y);
						//m_lights[m_lightsCount].LiveMapCell = liveMapCell;
		                m_lights[m_lightsCount++].LightSource = info.Source;
	                }

	                #endregion
                }

                #endregion

	            #region сформировать массив граней

	            m_edgesCount = 0;

	            for (var i = 1; i < MAP_SIZE - 1; ++i)
	            {
		            for (var j = 1; j < MAP_SIZE - 1; ++j)
		            {
			            var lmc = m_shadowCasters[i, j].LiveMapCell;
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
				                || m_shadowCasters[i + 1, j].Opacity != opacity
					            )
				            {
					            //const float d = 0.2f;
								var rect = new RectangleF(i, j, 1, 1);
								
								m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top))
					                                         {
						                                         Opacity = opacity,
						                                         TransparentColor = lmc.TransparentColor,
						                                         LiveMapCell = lmc
					                                         };
					            m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom))
					                                         {
						                                         Opacity = opacity,
						                                         TransparentColor = lmc.TransparentColor,
						                                         LiveMapCell = lmc
					                                         };

					            m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom))
					                                         {
						                                         Opacity = opacity,
						                                         TransparentColor = lmc.TransparentColor,
						                                         LiveMapCell = lmc
					                                         };
					            m_allEdges[m_edgesCount++] = new EdgeEx(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top))
					                                         {
						                                         Opacity = opacity,
						                                         TransparentColor = lmc.TransparentColor,
						                                         LiveMapCell = lmc
					                                         };
				            }
			            }
		            }
	            }

	            #endregion


	            //var dPoint = _liveMap.GetDPoint();
                //var viewportSize = _liveMap.VieportSize;

                while (m_fboBlit.CountOfBuffers < (m_lightsCount + 2))
                {
                    m_fboBlit.AddTextureBuffer();
                }

	            #region нарисовать зону видимости героя

	            //if (m_fboBlit.CountOfBuffers == 0)
				//{
				//	//m_fboBlit.AddTextureBuffer();
				//	using (new FboWrapper.DrawHelper(m_fbo))
				//	{
				//		GL.Color4(1f, 0f, 1f, 1f);
				//		GL.ClearColor(0f, 0f, 0f, 0f);
				//		GL.Clear(ClearBufferMask.ColorBufferBit);
				//		GL.Disable(EnableCap.Texture2D);

				//		GL.Begin(BeginMode.Polygon);
				//		{
				//			const int radius = MAP_SIZE/2;
				//			var center = MapBlock.GetInBlockCoords(World.TheWorld.Avatar.GeoInfo.LiveCoords) + new Point(32, 32);
                            
				//			GL.Color3(1f, 1f, 1f);
				//			GL.Vertex2(center.X, center.Y);
				//			GL.Color4(0f, 0f, 0f, 0f);
				//			const float step = (float) Math.PI/20f;
				//			for (float f = 0; f < Math.PI*2 + step; f += step)
				//			{
				//				var x = Math.Sin(f) * radius + center.X;
				//				var y = Math.Cos(f) * radius + center.Y;
				//				GL.Vertex2(x, y);
				//			}
				//			GL.Color3(1f, 1f, 1f);
				//			GL.Vertex2(center.X, center.Y);
				//		}
				//		GL.End();
				//		m_fbo.BlitTo(m_fboBlit, 0);
				//	}
				//}

	            #endregion




				var t = World.TheWorld.Avatar.GeoInfo.Layer.Ambient;
				for (var i = 0; i < m_lightsCount; i++)
	            {
		            using (new FboWrapper.DrawHelper(m_fbo))
		            {
						GL.ClearColor(t.R, t.G, t.B, 1.0f);
						GL.Clear(ClearBufferMask.ColorBufferBit);
						DrawShadows(m_lights[i]);
						m_fbo.BlitTo(m_fboBlit, i);
		            }
	            }
				m_fbo.BlitTo(m_fboBlit, m_lightsCount + 1);


                if (false)
                {
                    for(var a=0;a<m_lightsCount+1;++a)
                    {
                        using (new FboWrapper.DrawHelper(m_fboBlit))
                        {
                            GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + a);
                            Screenshot("d:\\bba" + a + ".png");
                        }
                    }
                }
                
                using (new FboWrapper.DrawHelper(m_fbo))
                {
                    GL.ClearColor(0f, 0f, 0f, 0f);
                    GL.Clear(ClearBufferMask.ColorBufferBit);
					GL.Color3(1f, 1f, 1f);
					GL.Enable(EnableCap.Texture2D);

                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcColor);

                    const float sz = MAP_SIZE;
                    const float to = ((float)MAP_SIZE) / FboWrapper.SIZE;

                    for (var a = 1; a < m_lightsCount; ++a)
                    {
                        m_fboBlit.BindTexture(a);
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

                    GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.OneMinusSrcAlpha);
                    m_fboBlit.BindTexture(0);
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

                    m_fbo.BlitTo(m_fboBlit, 1);
                }
                using (new FboWrapper.DrawHelper(m_fboBlit))
                {
                    GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
                    GL.ReadPixels(0, 0, MAP_SIZE, MAP_SIZE, PixelFormat.Rgba, PixelType.Float, m_lightMap);
                    //Screenshot("d:\\scr.png");

                    for (var i = 0; i < MAP_SIZE; i++)
                    {
                        for (var j = 0; j < MAP_SIZE; j++)
                        {
                            m_shadowCasters[j, i].LiveMapCell.Visibility = m_lightMap[i, j];
                            m_shadowCasters[j, i].LiveMapCell.Lighted = m_lightMap[i, j];
                            //m_shadowCasters[j, i].LiveMapCell.FinalLighted = FColor.White;// m_lightMap[i, j];
                            //m_shadowCasters[j, i].LiveMapCell.Lighted = 
                            //bmp.SetPixel(j, i,
                            //    Color.FromArgb((int)(arr[i, j].A * 255), (int)(arr[i, j].R * 255), (int)(arr[i, j].G * 255),
                            //        (int)(arr[i, j].B * 255)));
                        }
                    }

                    var layer = World.TheWorld.Avatar.GeoInfo.Layer;
	                var ambient = FColor.Black;// layer.Ambient;
                    var fogLightness = layer.FogLightness;

                    for (var i = 0; i < 9; i++)
                    {
                        var livBlockXY = liveBlocks[i, 1];
                        _liveMap.Blocks[livBlockXY.X, livBlockXY.Y].UpdateVisibility(fogLightness, ambient);
                    }
                    //Screenshot("e:\\scr.png");
                }
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            }
        }

        private static void Screenshot(string _filename)
        {
            var arr = new FColor[MAP_SIZE, MAP_SIZE];
            GL.ReadPixels(0, 0, MAP_SIZE, MAP_SIZE, PixelFormat.Rgba, PixelType.Float, arr);

            var bmp = new Bitmap(MAP_SIZE, MAP_SIZE, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            for (var i = 0; i < MAP_SIZE; i++)
            {
                for (var j = 0; j < MAP_SIZE; j++)
                {
                    bmp.SetPixel(j, i,
                        Color.FromArgb((int) (arr[i, j].A*255), (int) (arr[i, j].R*255), (int) (arr[i, j].G*255),
                            (int) (arr[i, j].B*255)));
                }
            }
            bmp.Save(_filename, ImageFormat.Png);
        }

        private void DrawShadows(Light _light)
		{
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			var lp = new PointF(_light.Point.X + 0.5f, _light.Point.Y + 0.5f);

			GL.Begin(BeginMode.Polygon);
			{
				GL.Color4(_light.LightSource.Color.R, _light.LightSource.Color.G, _light.LightSource.Color.B, 1f);

				GL.Vertex2(lp.X, lp.Y);
				GL.Color4(0f, 0f, 0f, 0f);
				const float step = (float)Math.PI / 10f;
				for (float f = 0; f < Math.PI * 2 + step; f += step)
				{
					var x = Math.Sin(f) * _light.LightSource.Radius + lp.X;
					var y = Math.Cos(f) * _light.LightSource.Radius + lp.Y;
					GL.Vertex2(x, y);
				}
				GL.Color4(_light.LightSource.Color.R, _light.LightSource.Color.G, _light.LightSource.Color.B, 1f);
				GL.Vertex2(lp.X, lp.Y);
			}
			GL.End();

			//return;

			#region Собираем все грани лицевые для источника освещения и попадающие в круг света

			var edges = new EdgeEx[MAP_SIZE * MAP_SIZE * 4];
			var edgesCount = 0;
			for (var i = 0; i < m_edgesCount; i++)
			{
				if (_light.LiveMapCell!=null && m_allEdges[i].LiveMapCell == _light.LiveMapCell) continue;
				if (EdgeEx.Distant(m_allEdges[i].P1, lp) >= _light.LightSource.Radius)continue;
				if (m_allEdges[i].Orient(lp) < 0) continue;
				edges[edgesCount] = m_allEdges[i];
				edges[edgesCount].Distance = Edge.Distant(m_allEdges[i].P1, lp);
				edges[edgesCount].Valid = true;
				edgesCount++;
			}

			Array.Sort(edges,0,edgesCount, new DistanceComparer());

			#endregion

			GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.OneMinusSrcAlpha);

			GL.Begin(BeginMode.Quads);
			{
				for (var i = 0; i < edgesCount; i++)
				{
					if (!edges[i].Valid) continue;

					GL.Color4(edges[i].TransparentColor.R, edges[i].TransparentColor.G, edges[i].TransparentColor.B, edges[i].Opacity);

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
					

					//continue;
					#region Отбрасываем все грани вошедшие внутрь теневой трапеции

					var e1 = new EdgeEx(pnt[0], pnt[3]);
					var e2 = new EdgeEx(pnt[2], pnt[1]);

					for (var j = i+1; j < edgesCount; j++)
					{
						//continue;
						if (!edges[j].Valid || edges[i].Opacity != edges[j].Opacity)
						{
							continue;
						}

					    #region 1

                        if (!(e1.Orient(edges[j].P2) < -float.Epsilon) || !(e2.Orient(edges[j].P1) < -float.Epsilon)) continue;
                        if (!(e1.Orient(edges[j].P1) < -float.Epsilon) || !(e2.Orient(edges[j].P2) < -float.Epsilon)) continue;

					    #endregion

                        #region 2

                        //if ((e1.Orient(edges[j].P2) > float.Epsilon) || (e2.Orient(edges[j].P1) > float.Epsilon)) continue;
                        //if ((e1.Orient(edges[j].P1) > float.Epsilon) || (e2.Orient(edges[j].P2) > float.Epsilon)) continue;

                        #endregion


					    edges[j].Valid = false;
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

	public class EdgeEx
	{
		public PointF P1;
		public PointF P2;

		public bool Valid;
		public float Distance;
		public float Opacity;

		public LiveMapCell LiveMapCell;

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
