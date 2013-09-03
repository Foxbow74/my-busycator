using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Shader
{
	public partial class Form1 : Form
	{
		private readonly Map m_map = new Map();
		
        private static Point _mousePnt;
		
        const float SZ = 3;

	    private const float LIGHTRADIUS = 20 * SZ;

		private readonly List<Edge> m_allEdges = new List<Edge>(Map.SIZE * Map.SIZE);
        
        // —мещение картинки на форме

	    private static PointF _pnt;

        private readonly Bitmap m_buf = new Bitmap(Map.SIZE * (int)SZ, Map.SIZE * (int)SZ, PixelFormat.Format32bppRgb);
			
		public Form1()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			
		}

		/// <summary>
		/// —ортировка по удаленности от источника света
		/// </summary>
		private class EdgeComparer:IComparer<Edge>
		{
			public int Compare(Edge _a, Edge _b)
			{
				var dista = (_pnt.X - _a.P1.X) * (_pnt.X - _a.P1.X) + (_pnt.Y - _a.P1.Y) * (_pnt.Y - _a.P1.Y);
				dista += (_pnt.X - _a.P2.X) * (_pnt.X - _a.P2.X) + (_pnt.Y - _a.P2.Y) * (_pnt.Y - _a.P2.Y);
				var distb = (_pnt.X - _b.P1.X) * (_pnt.X - _b.P1.X) + (_pnt.Y - _b.P1.Y) * (_pnt.Y - _b.P1.Y);
				distb += (_pnt.X - _b.P2.X) * (_pnt.X - _b.P2.X) + (_pnt.Y - _b.P2.Y) * (_pnt.Y - _b.P2.Y);
				return (int)((dista - distb)*1000);
			}
		}

		private void Form1_Paint(object _sender, PaintEventArgs _e)
		{
            m_allEdges.Clear();
            for (var i = 0; i < Map.SIZE; ++i)
            {
                for (var j = 0; j < Map.SIZE; ++j)
                {
                    if (m_map[i, j] > 0)
                    {
                        var rectEdges = GetRectEdges(new Rectangle((int)(i * SZ), (int)(j * SZ), (int)SZ, (int)SZ), m_map[i, j]);
                        if (j > 0 && m_map[i, j - 1] == 0)
                        {
                            m_allEdges.Add(rectEdges[0]);
                        }
                        if (i < (Map.SIZE - 1) && m_map[i + 1, j] == 0)
                        {
                            m_allEdges.Add(rectEdges[1]);
                        }
                        if (j < (Map.SIZE - 1) && m_map[i, j + 1] == 0)
                        {
                            m_allEdges.Add(rectEdges[2]);
                        }
                        if (i > 0 && m_map[i - 1, j] == 0)
                        {
                            m_allEdges.Add(rectEdges[3]);
                        }
                    }
                }
            }
            using (var g = Graphics.FromImage(m_buf))
		    {
                g.CompositingQuality=CompositingQuality.GammaCorrected;

                //g.SmoothingMode = SmoothingMode.HighQuality;
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                
                g.SetClip(new Rectangle(0, 0, Map.SIZE * (int)SZ, Map.SIZE * (int)SZ));
		        g.Clear(Color.Empty);

		        //for (int i = 0; i < 5; ++i)
		        {
                    Draw(_e, new PointF(_mousePnt.X * SZ, _mousePnt.Y * SZ), Color.FromArgb(255, 255, 255, 255), 0, g);
                    //Draw(_e, new PointF(_mousePnt.X, _mousePnt.Y), Color.FromArgb(255, 255, 180, 100), 0, g);
                }
                //Draw(_e, new PointF(_mousePnt.Y * SZ - m_offset.Y, _mousePnt.X * SZ - m_offset.X), Color.FromArgb(0, 255, 0), 0, g);
                //Draw(_e, new PointF(_mousePnt.Y * SZ - m_offset.Y, _mousePnt.Y * SZ - m_offset.Y), Color.FromArgb(0, 0, 255), 0, g);

                _e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                _e.Graphics.CompositingMode = CompositingMode.SourceCopy;
                _e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                _e.Graphics.DrawImage(m_buf, new RectangleF(0, 0, Map.SIZE, Map.SIZE), new RectangleF(0, 0, Map.SIZE * SZ, Map.SIZE * SZ), GraphicsUnit.Pixel);
                //_e.Graphics.DrawImageUnscaled(m_buf, 0,0);
            }
		    //Draw(_e, new PointF(_mousePnt.Y - m_offset.Y, _mousePnt.X - m_offset.X), Color.FromArgb(180, 255, 0), Map.SIZE);
            //Draw(_e, new PointF(_mousePnt.X - m_offset.X, _mousePnt.Y - m_offset.Y), Color.FromArgb(255, 180, 0), Map.SIZE * 2);
        }

	    private void Draw(PaintEventArgs _e, PointF p, Color lightColor, int o, Graphics g)
	    {
	            //g.SmoothingMode = SmoothingMode.HighQuality;
	            //g.InterpolationMode=InterpolationMode.HighQualityBicubic;


	            var gp = new GraphicsPath();
	            var tl = LIGHTRADIUS;
	            var rectOfLight = new RectangleF(p.X - tl, p.Y - tl, tl*2, tl*2);
	            gp.AddEllipse(rectOfLight);
	            using (var brush = new PathGradientBrush(gp))
	            {
	                brush.CenterColor = lightColor;
	                brush.SurroundColors = new[] {Color.Empty};
	                g.FillEllipse(brush, rectOfLight);
	            }

	            _pnt = p;
	            DrawShadows(g);
	            DrawWalls(g, m_buf);
	        
	    }

	    private void DrawShadows(Graphics g)
	    {
	        var edges = new List<Edge>(Map.SIZE*Map.SIZE);

	        #region —обираем все грани лицевые дл€ источника освещени€ и попадающие в круг света

	        edges.AddRange(from edge in m_allEdges
	            where Edge.Distant(edge.P1, _pnt) < LIGHTRADIUS*2 && edge.Orient(_pnt) >= 0
	            select new Edge(edge.P1, edge.P2) {Opacity = edge.Opacity});

	        #endregion

	        // —ортировка по удаленности от источника света
	        //edges.Sort(new EdgeComparer());


	        foreach (var edge in edges)
	        {
	            if (!edge.Valid)
	            {
	                continue;
	            }

	            edge.Valid = false;

	            #region –ассчитываем теневую трапецию

	            var pnt = new List<PointF>(new[] {edge.P2, edge.P1});
	            //bool flg;
	            //do
	            //{
	            //    flg = false;
	            //    foreach (var edge1 in edges)
	            //    {
	            //        if (!edge1.Valid)
	            //        {
	            //            continue;
	            //        }
	            //        if (edge1.P1 == edge.P2)
	            //        {
	            //            pnt.Insert(0, edge1.P2);
	            //            edge.P2 = edge1.P2;
	            //            edge1.Valid = false;
	            //            flg = true;
	            //        }
	            //        else if (edge1.P2 == edge.P1)
	            //        {
	            //            pnt.Add(edge1.P1);
	            //            edge.P1 = edge1.P1;
	            //            edge1.Valid = false;
	            //            flg = true;
	            //        }
	            //    }
	            //} while (flg);


	            var pntFar = new List<PointF>(pnt.Count);
	            for (var i = 0; i < pnt.Count; i++)
	            {
	                var f = pnt[pnt.Count - i - 1];
	                pntFar.Add(GetFarPnt(f));
	            }

	            pnt.AddRange(pntFar);

	            #endregion

                using (var brush = new SolidBrush(Color.FromArgb(edge.Opacity, Color.Black)))
	            {
	                g.FillPolygon(brush, pnt.ToArray());
	            }

	            #region ќтбрасываем все грани вошедшие внутрь теневой трапеции

	            var shadowEdges = new Edge[pnt.Count];
	            for (var i = 0; i < pnt.Count; ++i)
	            {
	                shadowEdges[i] = new Edge(pnt[i], pnt[(i + 1)%pnt.Count]);
	            }

	            foreach (var edge1 in edges.ToArray())
	            {
	                if (!edge1.Valid)
	                {
	                    continue;
	                }
	                var flag = true;
	                foreach (var shadowEdge in shadowEdges)
	                {
	                    if (!(shadowEdge.Orient(edge1.P1) > 0) && !(shadowEdge.Orient(edge1.P2) > 0)) continue;
	                    flag = false;
	                    break;
	                }
	                if (flag)
	                {
	                    edge1.Valid = false;
	                }
	            }

	            #endregion
	        }

	    }

	    /// <summary>
		/// ѕолучить точку на продолжении луча от источника света до аргумента
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		private static PointF GetFarPnt(PointF p)
		{
			var v = new Vector(_pnt, p);
            var md = LIGHTRADIUS * LIGHTRADIUS / v.Length;
            //var md = LIGHTRADIUS / v.Length;
			return v * md;
		}

	    /// <summary>
	    /// Subj
	    /// </summary>
	    /// <param name="g"></param>
	    /// <param name="_bitmap"></param>
	    private void DrawWalls(Graphics g, Bitmap _bitmap)
		{
			for (int i = 0; i < Map.SIZE; ++i)
			{
				for (int j = 0; j < Map.SIZE; ++j)
				{
					float mod = Edge.Distant(_pnt, new PointF(i*SZ + SZ/2, j*SZ + SZ/2));
					if (m_map[i, j] > 0 && mod < LIGHTRADIUS*1.1)
					{
						int alpha = 255 - (int)((float)255/LIGHTRADIUS * mod);
                        if (mod < 100)
                        {
                        }
						if (alpha < 0) 
						{
							alpha = 0;  
						}
                        if (alpha > 0)
                        {
                            _bitmap.SetPixel(i,j,Color.Black);
                        }
					}
				}
			}
		}

        private static Edge[] GetRectEdges(Rectangle rect, int _opacity)
        {
            return new[]
            {
                new Edge(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top)) {Opacity = _opacity},
                new Edge(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom)) {Opacity = _opacity},
                new Edge(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom)) {Opacity = _opacity},
                new Edge(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top)) {Opacity = _opacity},
            };
        }

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_mousePnt = e.Location;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			//Invalidate();
		}

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            int s = DateTime.Now.Second;
            int fps = 0;

            for (; ; )
            {
                if (closing || Disposing)
                {
                    return;
                }
                fps++;
                if(s != DateTime.Now.Second)
                {
                    s = DateTime.Now.Second;
                    label1.Text = string.Format("fps: {0}", fps);
                    fps = 0;
                }
                Invalidate();
                Application.DoEvents();
            }
        }

        private bool closing = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
        }
    }
}