using System;
using GameOfLife;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shader
{
    class FrameBuffer:IDisposable
    {
        const int SIZE = 512;
        private int m_fboId;
        private readonly List<TextureBuffer> m_buffers=new List<TextureBuffer>();

        public FrameBuffer()
        {
            // Create a FBO and attach the textures
            GL.Ext.GenFramebuffers(1, out m_fboId);
        }

        public int AddTextureBuffer()
        {
            var t = new TextureBuffer();
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext + m_buffers.Count, TextureTarget.Texture2D, t.TextureId, 0);
            m_buffers.Add(t);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
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

       public class DrawFBO:IDisposable
        {
            private readonly FrameBuffer m_frameBuffer;

            public DrawFBO(FrameBuffer _frameBuffer)
            {
                m_frameBuffer = _frameBuffer;
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_frameBuffer.m_fboId);
                GL.PushAttrib(AttribMask.ViewportBit); // stores GL.Viewport() parameters
                GL.Viewport(0, 0, SIZE, SIZE);
            }

            public void BeginDrawIn(int _i)
            {
                GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0Ext + _i);
            }

            public void EndDrawIn()
            {
            }

            public void Dispose()
            {
                GL.PopAttrib();
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // return to visible framebuffer
                GL.DrawBuffer(DrawBufferMode.Back);
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
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, SIZE, SIZE, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
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
        private const float LIGHTRADIUS = Map.SIZE/2*SZ;
        private readonly List<Edge> m_allEdges = new List<Edge>(Map.SIZE*Map.SIZE);
        private readonly Map m_map = new Map();

        private readonly FrameBuffer m_frameBuffer;
        private readonly int m_t1;

        public ShadowMap() : base(Map.SIZE*SZ, Map.SIZE*SZ, new GraphicsMode(32, 1, 1, 4))
        {
            m_frameBuffer = new FrameBuffer();
            m_t1 = m_frameBuffer.AddTextureBuffer();
            m_frameBuffer.Check();
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
            GL.ClearColor(0.9f, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            using (var dfbo = new FrameBuffer.DrawFBO(m_frameBuffer))
            {
                dfbo.BeginDrawIn(0);
                GL.ClearColor(1f, 0f, 0f, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
                GL.Disable(EnableCap.Texture2D);
                GL.Begin(BeginMode.Polygon);
                {
                    
                    GL.Color3(0f, 1f, 1f);
                    GL.Vertex2(Mouse.X, Mouse.Y);
                    GL.Color3(0, 0, 0);
                    const float step = (float)Math.PI / 10f;
                    for (float f = 0; f < Math.PI * 2 + step; f += step)
                    {
                        var x = Math.Sin(f) * 100 + Mouse.X;
                        var y = Math.Cos(f) * 100 + Mouse.Y;
                        GL.Vertex2(x, y);
                    }
                    GL.Color3(1f, 1f, 1f);
                    GL.Vertex2(Mouse.X, Mouse.Y);
                }
                GL.End();
                dfbo.EndDrawIn();
            }

            GL.UseProgram(0);
            GL.Enable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Quads);
            {
                GL.BindTexture(TextureTarget.ProxyTexture2D, m_t1);
                
                GL.TexCoord2(0f, 0f);
                GL.Vertex2(0f, 0f);

                GL.TexCoord2(1f, 0f);
                GL.Vertex2(512f, 0f);

                GL.TexCoord2(1f, 1f);
                GL.Vertex2(512f, 512f);

                GL.TexCoord2(0f, 1f);
                GL.Vertex2(0f, 512f);

                GL.BindTexture(TextureTarget.ProxyTexture2D, 0);
            }
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.Flush();
            //{
            //    GL.Begin(BeginMode.Polygon);
            //    {
            //        GL.Color3(1f, 1f, 1f);
            //        GL.Vertex2(Mouse.X, Mouse.Y);
            //        GL.Color3(0, 0, 0);
            //        const float step = (float) Math.PI/10f;
            //        for (float f = 0; f < Math.PI*2 + step; f += step)
            //        {
            //            var x = Math.Sin(f)*100 + Mouse.X;
            //            var y = Math.Cos(f)*100 + Mouse.Y;
            //            GL.Vertex2(x, y);
            //        }
            //        GL.Color3(1f, 1f, 1f);
            //        GL.Vertex2(Mouse.X, Mouse.Y);
            //    }
            //    GL.End();

            //    GL.Color3(0, 0, 0);
            //    GL.Begin(BeginMode.Quads);
            //    {
            //        DrawShadows(new PointF(Mouse.X, Mouse.Y));
            //        DrawShadows(new PointF(Mouse.Y, Mouse.Y));
            //        DrawShadows(new PointF(Mouse.X, Mouse.X));
            //    }
            //    GL.End();
            //}
            //GL.Flush();



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
            m_frameBuffer.Dispose();
        }

        [STAThread]
        public static void Main()
        {
            {
                try
                {
                    using (var game = new Game())
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


namespace GameOfLife
{
    class Game : GameWindow
    {
        int texture1, texture2, fragmentShaderHandle1, shaderProgramHandle1, FBOHandle1, FBOHandle2;
 
        bool evenFrame = false;
        bool tempo_counter = true;
        int size_w = 40;
        int size_h = 40;
        private double elapsedTime = 0;
        private bool slow = true;
 
        Random rnd = new Random();
 
        string fragmentShaderSource1 = @"
#version 400
uniform sampler2D MyTexture1;
uniform float pixel_w;
uniform float pixel_h;
bool isRed(vec2 coor);
 
void main(void)
{
// c is our current coordinate
// color is our current texture
 vec2 c = gl_TexCoord[0].xy;
 vec4 color = texture2D( MyTexture1, c);  
 
// keypad coordinates for neighbors -- ie: 8 => North, 3 => SE, etc...
 vec2 neighbor_1 = c;
 vec2 neighbor_2 = c;
 vec2 neighbor_3 = c;
 vec2 neighbor_4 = c;
 vec2 neighbor_6 = c;
 vec2 neighbor_7 = c;
 vec2 neighbor_8 = c;
 vec2 neighbor_9 = c;
 
// define neighbors
 neighbor_1.x -= pixel_w;
 neighbor_1.y -= pixel_h;
 
 neighbor_2.y -= pixel_h;
 
 neighbor_3.x += pixel_w;
 neighbor_3.y -= pixel_h;
 
 neighbor_4.x -= pixel_w;
 
 neighbor_6.x += pixel_h;
 
 neighbor_7.x -= pixel_w;
 neighbor_7.y += pixel_h;
 
 neighbor_8.y += pixel_h;
 
 neighbor_9.x += pixel_w;
 neighbor_9.y += pixel_h;
 
// calculate number of alive neighbors
 int count = 0;
 if (isRed(neighbor_1))
 {
    count++;
 }
 if (isRed(neighbor_2))
 {
    count++;
 }
 if (isRed(neighbor_3))
 {
    count++;
 }
 if (isRed(neighbor_4))
 {
    count++;
 }
 if (isRed(neighbor_6))
 {
    count++;
 }
 if (isRed(neighbor_7))
 {
    count++;
 }
 if (isRed(neighbor_8))
 {
    count++;
 }
 if (isRed(neighbor_9))
 {
    count++;
 }
 
 bool wasAlive = isRed(c);
 
 if (wasAlive){
  // if we dont have 2 or 3 neighbors, die
  if (count != 2 && count !=3){
    gl_FragColor.rgb = vec3(0.0, 0.0, 0.0);
    gl_FragColor.r = color.r * 0.49;
  }
  else{
    gl_FragColor.rgb = vec3(1.0, 0.0, 0.0);
  }
 }
 else{
  // spawn if 3 neighbors
 if (count == 3){
    gl_FragColor.rgb = vec3(1.0, 0.0, 0.0);
  }
 else{
    // fade away slowly
    gl_FragColor.rgb = vec3(0.0, 0.0, 0.0);
    gl_FragColor.r = color.r - 0.002;
  } 
 }
}
 
bool isRed(vec2 coor){
    vec4 color = texture2D( MyTexture1, coor );  
    if (color.r > 0.5){
        return true;
    }
    else{
        return false;
    }
}
 
";
        void CreateShaders()
        {
            // create shader
            fragmentShaderHandle1 = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle1, fragmentShaderSource1);
            GL.CompileShader(fragmentShaderHandle1);
            Debug.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle1));
 
            // Create program
            shaderProgramHandle1 = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle1, fragmentShaderHandle1);
            GL.LinkProgram(shaderProgramHandle1);
            Debug.WriteLine(GL.GetProgramInfoLog(shaderProgramHandle1));
        }
 
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
        }
 
        /// <summary>Load resources here.</sudfmmary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CreateShaders();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            int x = Width;
            CreateNullTexture(out texture1);
            CreateNullTexture(out texture2);
 
            CreateFBOandAssignTexture(out FBOHandle1, texture1);
            CreateFBOandAssignTexture(out FBOHandle2, texture2);
 
            // draw random points into the FBO that holds texture1 
            RenderRandomStartTextureInFBO(FBOHandle1);
 
            Console.WriteLine("Press F for fast clock, S for slow clock, Space to reseed textures");
        }
 
        private void CreateFBOandAssignTexture(out int fbo, int texture)
        {
            // create and bind an FBO
            GL.Ext.GenFramebuffers(1, out fbo);
            GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, fbo);
 
            // assign texture to FBO
            GL.Ext.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, texture, 0);
 
            #region Test for Error
 
            string version = string.Empty;
            try
            {
                //GLControl control = new GLControl();
                version = GL.GetString(StringName.Version);
            }
            catch (Exception ex)
            {
            }
 
            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                    {
                        Console.WriteLine("FBO: The framebuffer " + fbo + " is complete and valid for rendering.");
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
 
            #endregion Test for Error
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO
        }
 
        private void RenderRandomStartTextureInFBO(int FBOHandle)
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBOHandle);
            GL.PushAttrib(AttribMask.ViewportBit);
            {
                GL.Viewport(0, 0, size_w, size_h);
 
                // clear the screen in green, to make it very obvious what the clear affected. only the FBO, not the real framebuffer
                GL.ClearColor(0.0f, 1.0f, 0.0f, 1.0f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
 
                RenderRandomPoints();
            }
            GL.PopAttrib();
 
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO
        }
 
        private void RenderRandomPoints()
        {
            GL.PushAttrib(AttribMask.ColorBufferBit);
 
            // make sure no lingering textures are bound to draw vertices clearly.
            GL.BindTexture(TextureTarget.Texture2D, 0);
 
            GL.Begin(BeginMode.Points);
            {
                GL.Color3(1.0f, 0.0f, 0.0f);
                for (float x = 0; x <= size_w; x += 0.9f)
                {
                    for (float y = 0; y <= size_h; y+= 0.9f)
                    {
                        if (rnd.Next(2) == 0)
                            GL.Color3(1.0f, 0.0f, 0.0f);
                        else
                            GL.Color3(0.0f, 0.0f, 0.0f);
                        GL.Vertex2(-1.0 + 2 * x / (float)size_w, -1.0 + 2 * y / (float)size_h);
                    }
                }
            }
 
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.End();
            GL.PopAttrib();
        }
 
        private void CreateNullTexture(out int texture)
        {
            // load texture 
            GL.GenTextures(1, out texture);
 
            // Still required else TexImage2D will be applyed on the last bound texture
            GL.BindTexture(TextureTarget.Texture2D, texture);
 
            // generate null texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size_w, size_h, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
 
            // set filtering to nearest so we get "atari 2600" look
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }
 
        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
 
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
 
        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
 
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
 
            if (Keyboard[Key.Escape])
                Exit();
            if (Keyboard[Key.F])
            {
                slow = false;
            }
            if (Keyboard[Key.S])
            {
                slow = true;
            }
            if (Keyboard[Key.Space])
            {
                RenderRandomStartTextureInFBO(FBOHandle1);
                RenderRandomStartTextureInFBO(FBOHandle2);
            }
        }
 
        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(0.4f, 0.3f, 0.9f, 0f);
 
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
 
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 modelview = Matrix4.LookAt(0, 0, 1,
                                               0, 0, 0,
                                               0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
 
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1.1);
 
            float third = 2.0f / 3.0f;
 
            // draw a square that holds texture1 in upper left
            GL.UseProgram(0);
            GL.BindTexture(TextureTarget.Texture2D, texture1);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f + 2* third, 0.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f + 2* third + third, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f + third, -1.0f + 2* third + third, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f + third, -1.0f + 2*third, 0.0f);
            GL.End();
 
            // now render texture 2 in a square in upper right
            GL.UseProgram(0);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            GL.BindTexture(TextureTarget.Texture2D, texture2);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f + 2 * third, -1.0f + 2* third, 0.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f + 2 * third, -1.0f + 2* third + third, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f + third + 2 * third, -1.0f + 2* third + third, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f + third + 2 * third, -1.0f + 2* third, 0.0f);
            GL.End();
 
 
            GL.UseProgram(shaderProgramHandle1);
            GL.Uniform1(GL.GetUniformLocation(shaderProgramHandle1, "MyTexture1"), 0);
            GL.Uniform1(GL.GetUniformLocation(shaderProgramHandle1, "pixel_w"), 1.0f / size_w);
            GL.Uniform1(GL.GetUniformLocation(shaderProgramHandle1, "pixel_h"), 1.0f / size_h);
            GL.Viewport(0, 0, size_w, size_h);
 
            elapsedTime += e.Time;
 
            if (tempo())
            {
                if (!evenFrame)
                {
                    evenFrame = true;
                    // run texture2 through a shader, and set to texture1
                    GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBOHandle2);
                    GL.BindTexture(TextureTarget.Texture2D, texture1);
                    GL.Begin(BeginMode.Quads);
                    GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 0.0f);
                    GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 0.0f);
                    GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
                    GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 0.0f);
                    GL.End();
                    GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO
                }
            }
            else
            {
                if (evenFrame)
                {
                    evenFrame = false;
                    // run texture1 through a shader, and set to texture2
                    GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBOHandle1);
                    GL.BindTexture(TextureTarget.Texture2D, texture2);
                    GL.Begin(BeginMode.Quads);
                    GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 0.0f);
                    GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 0.0f);
                    GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
                    GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 0.0f);
                    GL.End();
                    GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO
                }
            }
 
            // to give final "life" effect, render in a square alternating between texture 1 and 2 in bottom center
            GL.UseProgram(0);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            if (evenFrame)
                GL.BindTexture(TextureTarget.Texture2D, texture2);
            else
                GL.BindTexture(TextureTarget.Texture2D, texture1);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f + 1 * third, -1.0f, 0.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f + 1 * third, -1.0f + third, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f + third + 1 * third, -1.0f + third, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f + third + 1 * third, -1.0f, 0.0f);
            GL.End();
 
            SwapBuffers();
    }
 
        private bool tempo()
        {
            tempo_counter = !tempo_counter;
            if (slow)
                return ((int)(elapsedTime) % 2 == 0);
            else
                return tempo_counter;
 
        }
 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
 
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
    }
}