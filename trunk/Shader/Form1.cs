using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using QuickFont;

namespace Shader
{
	public partial class Form1 : Form
	{
		private readonly Map m_map = new Map();
		
        private static Point _mousePnt;
		
        const float SZ = 3;

	    private const float LIGHTRADIUS = Map.SIZE/2*SZ;

		private readonly List<Edge> m_allEdges = new List<Edge>(Map.SIZE * Map.SIZE);
        
        // —мещение картинки на форме

	    private static PointF m_pnt;

		private readonly Bitmap m_buf0 = new Bitmap(Map.SIZE * (int)SZ, Map.SIZE * (int)SZ, PixelFormat.Format32bppPArgb);
		private readonly Bitmap m_buf1 = new Bitmap(Map.SIZE * (int)SZ, Map.SIZE * (int)SZ, PixelFormat.Format32bppPArgb);
		private readonly Bitmap m_buf2 = new Bitmap(Map.SIZE * (int)SZ, Map.SIZE * (int)SZ, PixelFormat.Format32bppPArgb);
		private readonly Bitmap m_buf3 = new Bitmap(Map.SIZE * (int)SZ, Map.SIZE * (int)SZ, PixelFormat.Format32bppPArgb);

		private QBitmap m_qb;
			
		public Form1()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			//m_qb = new QBitmap(m_buf3);
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

			#region зона видимости

			using (var g = Graphics.FromImage(m_buf3))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.Clear(Color.Empty);

				Draw(_e, new PointF(Map.SIZE * SZ / 2 , Map.SIZE * SZ / 2 ), Color.White, 0, g);
			}

			#endregion


			using (var g = Graphics.FromImage(m_buf1))
			{
				//g.CompositingQuality = CompositingQuality.GammaCorrected;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.Clear(Color.Empty);

				Draw(_e, new PointF(_mousePnt.X * SZ, _mousePnt.Y * SZ), Color.FromArgb(255, 255, 0, 0), 0, g);
			}
			using (var g = Graphics.FromImage(m_buf2))
			{
				//g.CompositingQuality = CompositingQuality.GammaCorrected;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.Clear(Color.Empty);

				Draw(_e, new PointF(_mousePnt.Y * SZ, _mousePnt.X * SZ), Color.FromArgb(255, 0, 0, 255), 0, g);
			}
			using (var g = Graphics.FromImage(m_buf0))
			{
				//g.CompositingQuality = CompositingQuality.GammaCorrected;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.Clear(Color.Empty);

				Draw(_e, new PointF(_mousePnt.Y * SZ, _mousePnt.Y * SZ), Color.FromArgb(255, 0, 255, 0), 0, g);
			}
			//DrawWalls(m_buf2);

			using (var t= new QBitmap(m_buf3))
			{
				t.ApplyLightMaps(new[] { m_buf0, m_buf1, m_buf2 });
			}

			//_e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			_e.Graphics.CompositingMode = CompositingMode.SourceCopy;
			//_e.Graphics.InterpolationMode = InterpolationMode.Bilinear;
			_e.Graphics.DrawImage(m_buf3, new RectangleF(0, 0, Map.SIZE , Map.SIZE ), new RectangleF(0, 0, Map.SIZE * SZ, Map.SIZE * SZ), GraphicsUnit.Pixel);
			
			//Draw(_e, new PointF(_mousePnt.Y - m_offset.Y, _mousePnt.X - m_offset.X), Color.FromArgb(180, 255, 0), Map.SIZE);
            //Draw(_e, new PointF(_mousePnt.X - m_offset.X, _mousePnt.Y - m_offset.Y), Color.FromArgb(255, 180, 0), Map.SIZE * 2);
        }

	    private void Draw(PaintEventArgs _e, PointF p, Color lightColor, int o, Graphics g)
	    {
	            //g.SmoothingMode = SmoothingMode.HighQuality;
	            //g.InterpolationMode=InterpolationMode.HighQualityBicubic;


            var gp = new GraphicsPath();
            var tl = LIGHTRADIUS;
            var rectOfLight = new RectangleF(p.X - tl, p.Y - tl, tl * 2, tl * 2);
            gp.AddEllipse(rectOfLight);
            using (var brush = new PathGradientBrush(gp))
            {
                brush.CenterColor = lightColor;
                brush.SurroundColors = new[] { Color.Empty };
                g.FillEllipse(brush, rectOfLight);
            }

	            m_pnt = p;
	            DrawShadows(g);
	    }

		private SolidBrush brush = new SolidBrush(Color.FromArgb(255, Color.Black));

	    private void DrawShadows(Graphics g)
	    {
	        var edges = new List<Edge>(Map.SIZE*Map.SIZE);

	        #region —обираем все грани лицевые дл€ источника освещени€ и попадающие в круг света

	        edges.AddRange(from edge in m_allEdges
	            where Edge.Distant(edge.P1, m_pnt) < LIGHTRADIUS*2 && edge.Orient(m_pnt) >= 0
	            select new Edge(edge.P1, edge.P2) {Opacity = edge.Opacity});

	        #endregion

	        // —ортировка по удаленности от источника света
	        //edges.Sort(new EdgeComparer());


	        foreach (var edge in edges)
	        {
				//if (!edge.Valid)
				//{
				//	continue;
				//}

				//edge.Valid = false;

				var pnt = new[] { edge.P2, edge.P1, GetFarPnt(edge.P1), GetFarPnt(edge.P2) };

	            {
					g.FillPolygon(brush, pnt);
	            }

	            #region ќтбрасываем все грани вошедшие внутрь теневой трапеции

				var shadowEdges = new Edge[4];
				for (var i = 0; i <4; ++i)
	            {
					shadowEdges[i] = new Edge(pnt[i], pnt[(i + 1) % 4]);
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
			var v = new Vector(m_pnt, p);
            var md = LIGHTRADIUS * LIGHTRADIUS / v.Length;
            //var md = LIGHTRADIUS / v.Length;
			return v * md;
		}

		/// <summary>
		/// Subj
		/// </summary>
		/// <param name="_bitmap"></param>
		private void DrawWalls(Bitmap _bitmap)
		{
			for (int i = 0; i < Map.SIZE; ++i)
			{
				for (int j = 0; j < Map.SIZE; ++j)
				{
					float mod = Edge.Distant(m_pnt, new PointF(i*SZ + SZ/2, j*SZ + SZ/2));
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

		

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
			var sw = new Stopwatch();
	        sw.Start();
			int s = sw.Elapsed.Seconds;
            int fps = 0;

            for (; ; )
            {
                if (closing || Disposing)
                {
                    return;
                }
                fps++;
				if (s != sw.Elapsed.Seconds)
                {
					s = sw.Elapsed.Seconds;
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