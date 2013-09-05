using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shader
{
    class FboWrapper:IDisposable
    {
	    public const int SIZE = 512;
        private int m_fboId;
        private readonly List<TextureBuffer> m_buffers=new List<TextureBuffer>();

        public FboWrapper()
        {
            // Create a FBO and attach the textures
            GL.Ext.GenFramebuffers(1, out m_fboId);
        }

        public int AddTextureBuffer()
        {
            var t = new TextureBuffer();
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboId);
			GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0 + m_buffers.Count, TextureTarget.Texture2D, t.TextureId, 0);
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

			m_buffers.Add(t);
			return t.TextureId;
        }

        public void Check()
        {
            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                    {
                        Console.WriteLine("FBO: The framebuffer is complete and valid for rendering.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                    {
                        Console.WriteLine("FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                    {
                        Console.WriteLine("FBO: There are no attachments.");
                        break;
                    }
                /* case  FramebufferErrorCode.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT: 
                     {
                         Console.WriteLine("FBO: An object has been attached to more than one attachment point.");
                         break;
                     }*/
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    {
                        Console.WriteLine("FBO: Attachments are of different size. All attachments must have the same width and height.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    {
                        Console.WriteLine("FBO: The color attachments have different format. All color attachments must have the same format.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    {
                        Console.WriteLine("FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    {
                        Console.WriteLine("FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferUnsupportedExt:
                    {
                        Console.WriteLine("FBO: This particular FBO configuration is not supported by the implementation.");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("FBO: Status unknown. (yes, this is really bad.)");
                        break;
                    }
            }
        }

        public class DrawHelper : IDisposable
        {
            private readonly FboWrapper m_fboWrapper;
            private readonly int[] m_savedViewport = new int[4];

            public DrawHelper(FboWrapper _fboWrapper)
            {
                m_fboWrapper = _fboWrapper;

                GL.Flush();
                GL.GetInteger(GetPName.Viewport, m_savedViewport);
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboWrapper.m_fboId);

                

                //GL.Viewport(0, 0, SIZE, SIZE);
                m_fboWrapper.Check();


                //GL.PushMatrix();

                //GL.MatrixMode(MatrixMode.Projection);
                //GL.PushMatrix();
                //GL.LoadIdentity();

                ////GL.LoadIdentity();
                //GL.MatrixMode(MatrixMode.Modelview);
                //GL.PushMatrix();
                //GL.LoadIdentity();
            }

            public void BeginDrawIn(int _i)
            {
                GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0 + _i);
                GL.BindTexture(TextureTarget.Texture2D, m_fboWrapper.m_buffers[_i].TextureId);

                //GL.PushMatrix();
                //GL.MatrixMode(MatrixMode.Texture);
                //GL.LoadIdentity();
                //GL.Ortho(0,SIZE,SIZE,0,-1,1);
            }

            public void EndDrawIn()
            {
                //GL.PopMatrix();
                //GL.PopMatrix();


                //GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            public void Dispose()
            {
                GL.Flush();
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                GL.Viewport(m_savedViewport[0], m_savedViewport[1], m_savedViewport[2], m_savedViewport[3]);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        class TextureBuffer
        {
            private readonly int m_textureId;

            public TextureBuffer()
            {
                //uint DepthRenderbuffer;

                // Create Color Texture
                GL.GenTextures(1, out m_textureId);
                GL.BindTexture(TextureTarget.Texture2D, TextureId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, SIZE, SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);

				GL.BindTexture(TextureTarget.Texture2D, 0); // prevent feedback, reading and writing to the same image is a bad idea
            }

            public int TextureId
            {
                get { return m_textureId; }
            }
        }

        public void Dispose()
        {
            foreach (var buffer in m_buffers)
            {
                var id = buffer.TextureId;
                GL.DeleteTextures(1, ref id);    
            }
            
            GL.Ext.DeleteFramebuffers(1, ref m_fboId);
        }
    }

   


    internal class ShadowMap : GameWindow
    {
        private const int SZ = 3;
	    private const float LIGHTRADIUS = 100;// Map.SIZE/2*SZ;
        private readonly List<Edge> m_allEdges = new List<Edge>(Map.SIZE*Map.SIZE);
        private readonly Map m_map = new Map();

        private readonly FboWrapper m_fboWrapper;
        private readonly int m_t1, m_t2;

	    public ShadowMap() : base(512, 512, new GraphicsMode(32, 1, 1, 4))
        {
            m_fboWrapper = new FboWrapper();
			m_t1 = m_fboWrapper.AddTextureBuffer();
			m_t2 = m_fboWrapper.AddTextureBuffer();
            m_fboWrapper.Check();
        }

        private static Edge[] GetRectEdges(Rectangle rect, int _opacity)
        {
            return new[]
            {
                new Edge(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top)) {Opacity = _opacity},
                new Edge(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom)) {Opacity = _opacity},
                new Edge(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom)) {Opacity = _opacity},
                new Edge(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top)) {Opacity = _opacity}
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
		    GL.ClearColor(1f, 1f, 1f, 1f);
		    GL.Clear(ClearBufferMask.ColorBufferBit);
			using (var dfbo = new FboWrapper.DrawHelper(m_fboWrapper))
			{
			    #region #1

			    dfbo.BeginDrawIn(0);
			    GL.ClearColor(1f, 1f, 1f, 0f);
			    GL.Clear(ClearBufferMask.ColorBufferBit);
			    GL.Disable(EnableCap.Texture2D);


			    GL.Begin(BeginMode.Polygon);
			    {

			        GL.Color3(1f, 1f, 0.7f);
			        //GL.Vertex2(Mouse.X, Mouse.Y);
			        GL.Vertex2(FboWrapper.SIZE/2, FboWrapper.SIZE/2);
			        GL.Color3(1f, 1f, 1f);
			        const float step = (float) Math.PI/10f;
			        for (float f = 0; f < Math.PI*2 + step; f += step)
			        {
			            var x = Math.Sin(f)*LIGHTRADIUS + FboWrapper.SIZE/2;
			            var y = Math.Cos(f)*LIGHTRADIUS + FboWrapper.SIZE/2;
			            GL.Vertex2(x, y);
			        }
			        GL.Color3(1f, 1f, 1f);
			        //GL.Vertex2(Mouse.X, Mouse.Y);
			        GL.Vertex2(FboWrapper.SIZE/2, FboWrapper.SIZE/2);

			    }
			    GL.End();
			    dfbo.EndDrawIn();

			    #endregion

			    #region #2

			    dfbo.BeginDrawIn(1);

			    GL.ClearColor(1f, 1f, 1f, 0f);
			    GL.Clear(ClearBufferMask.ColorBufferBit);
			    GL.Color3(1f, 0f, 0f);
			    GL.Begin(BeginMode.Lines);
			    {
			        GL.Vertex2(10f, 10f);
			        GL.Color3(0f, 0f, 1f);
			        GL.Vertex2(502f, 502f);
			    }
			    GL.End();

			    dfbo.EndDrawIn();

			    #endregion

                #region DrawShadows

                //dfbo.BeginDrawIn(1);
			    //GL.ClearColor(0f, 0f, 0f, 0f);
			    //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
			    //GL.Disable(EnableCap.Texture2D);
			    //GL.Color3(1f, 0, 1f);
			    //GL.Begin(BeginMode.Quads);
			    //{
			    //	DrawShadows(new PointF(Mouse.X, Mouse.Y));
			    //	DrawShadows(new PointF(Mouse.Y, Mouse.Y));
			    //	DrawShadows(new PointF(Mouse.X, Mouse.X));
			    //}
			    //GL.End();

			    #endregion

			}
            
            GL.BindTexture(TextureTarget.Texture2D, m_t1);
            GL.Color3(1f, 1f, 1f);
			GL.Enable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Quads);
            {
                GL.TexCoord2(0f, 0f);
                GL.Vertex2(0f, 0f);

                GL.TexCoord2(1f, 0f);
                GL.Vertex2(512f, 0f);

                GL.TexCoord2(1f, 1f);
                GL.Vertex2(512f, 512f);

                GL.TexCoord2(0f, 1f);
                GL.Vertex2(0f, 512f);

				GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            GL.End();
            GL.Disable(EnableCap.Texture2D);

			GL.Color3(0f, 0f, 0f);
			GL.Begin(BeginMode.Lines);
			{
				GL.Vertex2(10f, 10f);
				GL.Color3(0f, 1f, 1f);
				GL.Vertex2(502f, 502f);
			}
			GL.End();
            GL.Flush();

            SwapBuffers();
            Title = string.Format("fps:{0} per frame", Math.Round(1/_e.Time));
        }

        private void DrawShadows(PointF _pnt)
        {
            #region Собираем все грани лицевые для источника освещения и попадающие в круг света

             var edges = (from edge in m_allEdges
                where Edge.Distant(edge.P1, _pnt) < LIGHTRADIUS*2 && edge.Orient(_pnt) >= 0
                select new Edge(edge.P1, edge.P2) {Opacity = edge.Opacity}).ToArray();

            #endregion

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

                var shadowEdges = new Edge[4];
                for (var i = 0; i < 4; ++i)
                {
                    shadowEdges[i] = new Edge(pnt[i], pnt[(i + 1)%4]);
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
        ///     Получить точку на продолжении луча от источника света до аргумента
        /// </summary>
        /// <param name="_pnt"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static PointF GetFarPnt(PointF _pnt, PointF p)
        {
            var v = new Vector(_pnt, p);
            var md = LIGHTRADIUS*LIGHTRADIUS/v.Length;
            //var md = LIGHTRADIUS / v.Length;
            return v*md;
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            m_fboWrapper.Dispose();
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
