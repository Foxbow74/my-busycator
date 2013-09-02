using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Shader
{
	public partial class Form1 : Form
	{
		private readonly Map _map = new Map();
		
        private static Point _mousePnt = new Point();
		
        const int SZ = 5;
		
        private const float WV = SZ/10;
		
        private const int LIGHTRADIUS = 20 * SZ;

		private readonly List<Edge> _allEdges = new List<Edge>(Map.SIZE * Map.SIZE);
        
        // —мещение картинки на форме
        private readonly Point OFFSET = new Point(30, 30);

        private static PointF _pnt;

        private readonly Bitmap _buf = new Bitmap(SZ * Map.SIZE, SZ * Map.SIZE, PixelFormat.Format32bppRgb);
			
		public Form1()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			
			for (int i = 0; i < Map.SIZE; ++i)
			{
				for (int j = 0; j < Map.SIZE; ++j)
				{
					if (_map[i, j] > 0)
					{
						Edge[] rectEdges = GetRectEdges(new Rectangle(i*SZ, j*SZ, SZ, SZ));
						if (j > 0 && _map[i, j - 1] == 0)
						{
							_allEdges.Add(rectEdges[0]);
						}
						if (i < (Map.SIZE-1) && _map[i + 1, j] == 0)
						{
							_allEdges.Add(rectEdges[1]);
						}
						if (j < (Map.SIZE-1) && _map[i, j + 1] == 0)
						{
							_allEdges.Add(rectEdges[2]);
						}
						if (i > 0 && _map[i - 1, j] == 0)
						{
							_allEdges.Add(rectEdges[3]);
						}
					}
				}
			}
		}

		/// <summary>
		/// —ортировка по удаленности от источника света
		/// </summary>
		private class EdgeComparer:IComparer<Edge>
		{
			public int Compare(Edge a, Edge b)
			{
				float dista = (_pnt.X - a.P1.X) * (_pnt.X - a.P1.X) + (_pnt.Y - a.P1.Y) * (_pnt.Y - a.P1.Y);
				dista += (_pnt.X - a.P2.X) * (_pnt.X - a.P2.X) + (_pnt.Y - a.P2.Y) * (_pnt.Y - a.P2.Y);
				float distb = (_pnt.X - b.P1.X) * (_pnt.X - b.P1.X) + (_pnt.Y - b.P1.Y) * (_pnt.Y - b.P1.Y);
				distb += (_pnt.X - b.P2.X) * (_pnt.X - b.P2.X) + (_pnt.Y - b.P2.Y) * (_pnt.Y - b.P2.Y);
				return (int)((dista - distb)*1000);
			}
		}

        private float _alfa = 0;

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
            using (Graphics g = Graphics.FromImage(_buf))
            {
                g.SetClip(new Rectangle(0, 0, SZ*Map.SIZE, SZ*Map.SIZE));
                g.Clear(Color.Black);
                g.SmoothingMode = SmoothingMode.HighQuality;

                float d = (float)Map.rnd.NextDouble();
                float salfa = (float)Math.Sin(_alfa)*d;
                float calfa = (float)Math.Cos(_alfa)*d;
                _alfa += d;

                PointF p =
                    new PointF(_mousePnt.X - OFFSET.X + salfa * WV,
                               _mousePnt.Y - OFFSET.Y + calfa * WV);

                GraphicsPath gp = new GraphicsPath();
                float tl = LIGHTRADIUS * (1 - calfa / 30);
                RectangleF rectOfLight = new RectangleF(p.X - tl, p.Y - tl, tl * 2, tl * 2);
                gp.AddEllipse(rectOfLight);
                using (PathGradientBrush brush = new PathGradientBrush(gp))
                {
                    brush.CenterColor = Color.FromArgb(255, 180 - (int)(salfa*10), 0);
                    brush.SurroundColors = new Color[] { Color.Black };
                    g.FillEllipse(brush, rectOfLight);
                }

                _pnt = p;
                DrawShadows(g);
                DrawWalls(g);
            }
            e.Graphics.DrawImageUnscaled(_buf, OFFSET);
		}

		private void DrawShadows(Graphics g)
		{
			List<Edge> edges = new List<Edge>(Map.SIZE*Map.SIZE);

			#region —обираем все грани лицевые дл€ источника освещени€ и попадающие в круг света

			foreach (Edge edge in _allEdges)
			{
				if (Edge.Distant(edge.P1, _pnt) < LIGHTRADIUS && edge.Orient(_pnt) < 0)
				{
					edges.Add(new Edge(edge.P1, edge.P2));
				}
			}

			#endregion

			// —ортировка по удаленности от источника света
			edges.Sort(new EdgeComparer());

			using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, Color.Black)))
			{
				foreach (Edge edge in edges)
				{
					if (!edge.Valid)
					{
						continue;
					}

					edge.Valid = false;

					#region –ассчитываем теневую трапецию

					List<PointF> pnt = new List<PointF>(new PointF[] {edge.P2, edge.P1});
					bool flg;
					do
					{
						flg = false;
						foreach (Edge edge1 in edges)
						{
							if (!edge1.Valid)
							{
								continue;
							}
							if (edge1.P1 == edge.P2)
							{
								pnt.Insert(0, edge1.P2);
								edge.P2 = edge1.P2;
								edge1.Valid = false;
								flg = true;
							}
							else if (edge1.P2 == edge.P1)
							{
								pnt.Add(edge1.P1);
								edge.P1 = edge1.P1;
								edge1.Valid = false;
								flg = true;
							}
						}
					} while (flg);


					List<PointF> pntFar = new List<PointF>(pnt.Count);
					for (int i = 0; i < pnt.Count; i++)
					{
						PointF f = pnt[pnt.Count - i - 1];
						pntFar.Add(GetFarPnt(f));
					}

					pnt.AddRange(pntFar);

					#endregion

					g.FillPolygon(brush, pnt.ToArray());

					#region ќтбрасываем все грани вошедшие внутрь теневой трапеции

					Edge[] shadowEdges = new Edge[pnt.Count];
					for (int i = 0; i < pnt.Count; ++i)
					{
						shadowEdges[i]=new Edge(pnt[i], pnt[(i + 1) % pnt.Count]);
					}

					foreach (Edge edge1 in edges.ToArray())
					{
						if (!edge1.Valid)
						{
							continue;
						}
						bool flag = true;
						foreach (Edge shadowEdge in shadowEdges)
						{
							if (shadowEdge.Orient(edge1.P1) > 0 || shadowEdge.Orient(edge1.P2) > 0)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							edge1.Valid = false;
						}
					}

					#endregion
				}
			}
		}

		/// <summary>
		/// ѕолучить точку на продолжении луча от источника света до аргумента
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		private static PointF GetFarPnt(PointF p)
		{
			Vector v = new Vector(_pnt, p);
			float md = LIGHTRADIUS * SZ * SZ / v.Length;
			return v * md;
		}

		/// <summary>
		/// Subj
		/// </summary>
		/// <param name="g"></param>
		private void DrawWalls(Graphics g)
		{
			for (int i = 0; i < Map.SIZE; ++i)
			{
				for (int j = 0; j < Map.SIZE; ++j)
				{
					float mod = Edge.Distant(_pnt, new PointF(i*SZ + SZ/2, j*SZ + SZ/2));
					if (_map[i, j] > 0 && mod < LIGHTRADIUS*1.1)
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
                            using (Brush br = new SolidBrush(Color.FromArgb(alpha, Color.FromArgb(100, 60, 0))))
                            {
                                Rectangle rect = new Rectangle(i * SZ, j * SZ, SZ, SZ);
                                g.FillRectangle(br, rect);
                            }
                        }
					}
				}
			}
		}

		private static Edge[] GetRectEdges(Rectangle rect)
		{
			return new Edge[]
				{
					new Edge(new PointF(rect.Left,rect.Top), new PointF(rect.Right, rect.Top) ), 
					new Edge(new PointF(rect.Right,rect.Top), new PointF(rect.Right, rect.Bottom) ), 
					new Edge(new PointF(rect.Right,rect.Bottom), new PointF(rect.Left, rect.Bottom) ), 
					new Edge(new PointF(rect.Left,rect.Bottom), new PointF(rect.Left, rect.Top) ), 
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