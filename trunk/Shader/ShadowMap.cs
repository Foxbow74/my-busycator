using System;
using System.Drawing.Imaging;
using GameCore;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Shader
{
    internal class ShadowMap : GameWindow
    {
        private const int VSZ = 1;
        private const int SZ = 1;
	    private const float LIGHTRADIUS = Map.SIZE/3;// Map.SIZE/2*SZ;
        private readonly List<Edge> m_allEdges = new List<Edge>(Map.SIZE*Map.SIZE);
        private readonly List<Point> m_allWalls = new List<Point>(Map.SIZE * Map.SIZE);
        private readonly Map m_map = new Map();

        private readonly FboWrapper m_fboWrapperBlit;
        private readonly FboWrapper m_fboWrapper;

        private readonly int m_t1, m_t2;
        private int m_shaderProgramHandle;
        private int m_fragmentShaderHandle;

        private const string FRAGMENT_SHADER_SOURCE = @"
 
#version 120
    uniform sampler2D Spot;
    uniform sampler2D Shadow0;
    uniform sampler2D Shadow1;
void main(void)
{
    vec2 xy = gl_TexCoord[0].st;
    vec4 c = texture2D( Spot, xy );
    vec4 s0 = texture2D( Shadow0, xy );
	gl_FragColor = c*s0;
}";

        public ShadowMap()
            : base(Map.SIZE * VSZ, Map.SIZE * VSZ)
        {
            if (SZ*Map.SIZE > FboWrapper.SIZE)
            {
                throw new ApplicationException();
            }

            CreateShaders();

            m_fboWrapperBlit = new FboWrapper(false);
			m_t1 = m_fboWrapperBlit.AddTextureBuffer();
            m_t2 = m_fboWrapperBlit.AddTextureBuffer();
            m_fboWrapperBlit.Check();

            m_fboWrapper = new FboWrapper(true);
			//m_fboWrapper.AddTextureBuffer();

			VSync=VSyncMode.Off;
        }

        private static Edge[] GetRectEdges(Rectangle _rect, int _opacity)
        {
            return new[]
            {
                new Edge(new PointF(_rect.Left, _rect.Top), new PointF(_rect.Right, _rect.Top)) {Opacity = _opacity},
                new Edge(new PointF(_rect.Right, _rect.Top), new PointF(_rect.Right, _rect.Bottom)) {Opacity = _opacity},
                new Edge(new PointF(_rect.Right, _rect.Bottom), new PointF(_rect.Left, _rect.Bottom)) {Opacity = _opacity},
                new Edge(new PointF(_rect.Left, _rect.Bottom), new PointF(_rect.Left, _rect.Top)) {Opacity = _opacity}
            };
        }

        protected override void OnLoad(EventArgs _e)
        {
            base.OnLoad(_e);

			var viewPort = new int[4];

			GL.GetInteger(GetPName.Viewport, viewPort);

			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();

			GL.Ortho(viewPort[0], viewPort[0] + viewPort[2], viewPort[1] + viewPort[3], viewPort[1], -1, 1);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();

			GL.PushAttrib(AttribMask.DepthBufferBit);
			GL.Disable(EnableCap.DepthTest);

			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);

            GL.ClearColor(0f, 0f, 0f, 0f);
            //GL.ShadeModel(ShadingModel.Smooth);

            m_allEdges.Clear();
            for (var i = 0; i < Map.SIZE; ++i)
            {
                for (var j = 0; j < Map.SIZE; ++j)
                {
                    if (m_map[i, j] > 0)
                    {
                        m_allWalls.Add(new Point(i,j));
                        var rectEdges = GetRectEdges(new Rectangle(i*SZ, j*SZ, SZ, SZ), m_map[i, j]);
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
        }

	    protected override void OnRenderFrame(FrameEventArgs _e)
	    {
		    base.OnRenderFrame(_e);
		    GL.ClearColor(0f, 0f, 0f, 0f);
		    GL.Clear(ClearBufferMask.ColorBufferBit);
            using (new FboWrapper.DrawHelper(m_fboWrapper))
            {
                #region #1

	            //dfbo.BeginDrawIn(0);
	            GL.ClearColor(0f, 0f, 0f, 0f);
	            GL.Clear(ClearBufferMask.ColorBufferBit);
	            GL.Disable(EnableCap.Texture2D);


	            GL.Begin(BeginMode.Polygon);
	            {
	                const int center = Map.SIZE*SZ/2;

	                GL.Color3(1f, 1f, 0.7f);
	                GL.Vertex2(center, center);
	                GL.Color3(0f, 0f, 0f);
	                const float step = (float) Math.PI/10f;
	                for (float f = 0; f < Math.PI*2 + step; f += step)
	                {
	                    var x = Math.Sin(f)*LIGHTRADIUS + center;
	                    var y = Math.Cos(f)*LIGHTRADIUS + center;
	                    GL.Vertex2(x, y);
	                }
	                GL.Color3(1f, 1f, 0.7f);
	                GL.Vertex2(center, center);
	            }
	            GL.End();

	            #endregion
                m_fboWrapper.BlitTo(m_fboWrapperBlit, 0);
            }

	        using (new FboWrapper.DrawHelper(m_fboWrapper))
	        {

                DrawShadows(new PointF(Mouse.X * SZ / VSZ, Mouse.Y * SZ / VSZ));
	            m_fboWrapper.BlitTo(m_fboWrapperBlit, 1);
	        }

			const float sz = Map.SIZE * VSZ;//Map.SIZE; //512f / SZ;
			const float to = ((float)Map.SIZE * SZ) / FboWrapper.SIZE;


			if (true)
			{
				using (new FboWrapper.DrawHelper(m_fboWrapper))
				{
					GL.Color4(1f, 1f, 1f, 1f);
					GL.Enable(EnableCap.Texture2D);
					m_fboWrapperBlit.BindTexture(1);

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

					GL.BindTexture(TextureTarget.ProxyTexture2D, 0);
					GL.Disable(EnableCap.Texture2D);


					var arr = new FColor[Map.SIZE, Map.SIZE];
					GL.ReadPixels(0, 0, Map.SIZE, Map.SIZE, PixelFormat.Rgba, PixelType.Float, arr);

					var bmp = new Bitmap(Map.SIZE, Map.SIZE, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
					for (int i = 0; i < Map.SIZE; i++)
					{
						for (int j = 0; j < Map.SIZE; j++)
						{
							bmp.SetPixel(j, i,
								Color.FromArgb((int) (arr[i, j].A*255), (int) (arr[i, j].R*255), (int) (arr[i, j].G*255),
									(int) (arr[i, j].B*255)));
						}
					}
					bmp.Save("d:\\eee.png", ImageFormat.Png);
				}
			}


	        GL.UseProgram(m_shaderProgramHandle);

            BindTexture(m_t2, TextureUnit.Texture1, "Shadow0");
            BindTexture(m_t1, TextureUnit.Texture0, "Spot");


            GL.Color3(1f, 1f, 1f);

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
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
            
            GL.UseProgram(0);

            GL.Flush();

			
			//var arr = new FColor[Map.SIZE, Map.SIZE];
			//GL.ReadPixels(0, 0, Map.SIZE, Map.SIZE, PixelFormat.Rgba, PixelType.Float, arr);

			//var bmp = new Bitmap(Map.SIZE, Map.SIZE, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			//for (int i = 0; i < Map.SIZE; i++)
			//{
			//	for (int j = 0; j < Map.SIZE; j++)
			//	{
			//		bmp.SetPixel(j, i, Color.FromArgb((int)(arr[i, j].A * 255), (int)(arr[i, j].R * 255), (int)(arr[i, j].G * 255), (int)(arr[i, j].B * 255)));
			//	}
			//}
			//bmp.Save("d:\\aaa.png",ImageFormat.Png);

            SwapBuffers();

            Title = string.Format("fps:{0} per frame", Math.Round(1/_e.Time));
        }



        void CreateShaders()
        {

            m_fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(m_fragmentShaderHandle, FRAGMENT_SHADER_SOURCE);
            GL.CompileShader(m_fragmentShaderHandle);
            Debug.WriteLine(GL.GetShaderInfoLog(m_fragmentShaderHandle));

            // Create program
            m_shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(m_shaderProgramHandle, m_fragmentShaderHandle);
            GL.LinkProgram(m_shaderProgramHandle);
            Debug.WriteLine(GL.GetProgramInfoLog(m_shaderProgramHandle));
        }

        private void BindTexture(int _textureId, TextureUnit _textureUnit, string _uniformName)
        {
            GL.ActiveTexture(_textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.Uniform1(GL.GetUniformLocation(m_shaderProgramHandle, _uniformName), _textureUnit - TextureUnit.Texture0);
        }

        private void DrawShadows(PointF _pnt)
        {
            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
                
            GL.Begin(BeginMode.Polygon);
            {

                GL.Color3(1f, 0.7f, 0.7f);
                
                GL.Vertex2(_pnt.X, _pnt.Y);
                GL.Color3(0f, 0f, 0f);
                const float step = (float) Math.PI/10f;
                for (float f = 0; f < Math.PI*2 + step; f += step)
                {
                    var x = Math.Sin(f)*LIGHTRADIUS + _pnt.X;
                    var y = Math.Cos(f)*LIGHTRADIUS + _pnt.Y;
                    GL.Vertex2(x, y);
                }
                GL.Color3(1f, 0.7f, 1f);
                GL.Vertex2(_pnt.X, _pnt.Y);
            }
            GL.End();

            #region Собираем все грани лицевые для источника освещения и попадающие в круг света

            var edges = (from edge in m_allEdges
                where Edge.Distant(edge.P1, _pnt) < LIGHTRADIUS*2 && edge.Orient(_pnt) >= 0
				orderby Edge.Distant(edge.P1, _pnt)
                select new Edge(edge.P1, edge.P2) {Opacity = edge.Opacity,Distance = Edge.Distant(edge.P1, _pnt)}
				
				).ToArray();

            #endregion

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);
            {
                GL.Color3(0f, 0f, 0f);
                //GL.Color4(0f, 0f, 0f,0.1f);
                foreach (var edge in edges)
                {
                    if (!edge.Valid) continue;
                    var pnt = new[] {edge.P2, edge.P1, GetFarPnt(_pnt, edge.P1), GetFarPnt(_pnt, edge.P2)};
                    {
                        GL.Vertex2(pnt[0].X, pnt[0].Y);
                        GL.Vertex2(pnt[1].X, pnt[1].Y);
                        GL.Vertex2(pnt[2].X, pnt[2].Y);
                        GL.Vertex2(pnt[3].X, pnt[3].Y);
                    }

                    #region Отбрасываем все грани вошедшие внутрь теневой трапеции

					var e1 = new Edge(pnt[2], pnt[0]);
					var e2 = new Edge(pnt[0], pnt[3]);

					foreach (var edge1 in edges)
					{
						if (!edge1.Valid)
						{
							continue;
						}
						if (e1.Orient(edge1.P1) < 0 && e2.Orient(edge1.P1) < 0)
						{
							edge1.Valid = false;
						}
					}

                    #endregion
                }
            }
            GL.End();
            GL.Disable(EnableCap.Blend);
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
            var md = LIGHTRADIUS*LIGHTRADIUS/v.Length;
            //var md = LIGHTRADIUS / v.Length;
            return v*md;
        }

        protected override void OnUnload(EventArgs _e)
        {
            base.OnUnload(_e);
            m_fboWrapperBlit.Dispose();
        }

        [STAThread]
        public static void Main()
        {
            {
                try
                {
                    using (var game = new ShadowMap())
                    {
                        game.Run(0, 0);
                    }
                }
                catch (Exception exception)
                {
                    var path = Path.Combine(Environment.CurrentDirectory, "error_file.txt");
                    File.Delete(path);
                    File.AppendAllText(path, exception.Message, Encoding.Unicode);
                    File.AppendAllText(path, exception.StackTrace, Encoding.Unicode);
                    Process.Start("error_file.txt");
                }
            }
        }
    }
}
