using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Shader
{
    class FboWrapper:IDisposable
    {
        public const int SIZE = 1024;
        private int m_fboId;
        private readonly List<ColorBuffer> m_buffers=new List<ColorBuffer>();
        private int m_color_rb;
        private const int SAMPLES=4;

        public FboWrapper()
        {
            GL.Ext.GenFramebuffers(1, out m_fboId);
            Check();
        }

        public void AddRenderBuffer()
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboId);
            GL.Ext.GenRenderbuffers(1, out m_color_rb);
            GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, m_color_rb);
            GL.Ext.RenderbufferStorageMultisample((ExtFramebufferMultisample)(int)RenderbufferTarget.RenderbufferExt, SAMPLES, (ExtFramebufferMultisample)(int)PixelFormat.Rgba, SIZE, SIZE);
            GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.RenderbufferExt, m_color_rb);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            Check();
        }

        public int AddTextureBuffer()
        {
            var t = new ColorBuffer();
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0 + m_buffers.Count, TextureTarget.Texture2D, t.TextureId, 0);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

            m_buffers.Add(t);
            Check();
            return t.TextureId;
        }

        public void Check()
        {
            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                {
                   // Console.WriteLine("FBO: The framebuffer is complete and valid for rendering.");
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

        public void BeginDrawIn(int _i)
        {
            //GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext + _i, RenderbufferTarget.RenderbufferExt, m_fboWrapper.m_color_rb);
            //GL.ActiveTexture(TextureUnit.Texture0+_i);
            GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0Ext + _i);
            GL.BindTexture(TextureTarget.Texture2D, m_buffers[_i].TextureId);
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

                

                var viewPort = new int[4];
                GL.GetInteger(GetPName.Viewport, viewPort);

                GL.PushAttrib(AttribMask.ViewportBit);
                GL.Viewport(0,0,SIZE,SIZE);

                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix(); 
                GL.LoadIdentity();
                GL.Ortho(0, SIZE, SIZE, 0, -1, 1);
                GL.Scale(1f, -1f, 1f);
                GL.Translate(0f, -SIZE, 0f);

                //m_fboWrapper.Check();
                GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, m_fboWrapper.m_color_rb);
            }

           

            public void Dispose()
            {
                GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, 0);
                GL.Flush();
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.PopAttrib();

                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                GL.Viewport(m_savedViewport[0], m_savedViewport[1], m_savedViewport[2], m_savedViewport[3]);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        class ColorBuffer
        {
            private readonly int m_textureId;

            public ColorBuffer()
            {
                GL.GenTextures(1, out m_textureId);
                GL.BindTexture(TextureTarget.Texture2D, TextureId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, SIZE, SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
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
            GL.Ext.DeleteRenderbuffers(1, ref m_color_rb);
            GL.Ext.DeleteFramebuffers(1, ref m_fboId);
        }

        public void BlitTo(FboWrapper _fboWrapperBlit, int _attachment)
        {

            //blitting
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _fboWrapperBlit.m_fboId);
            _fboWrapperBlit.BeginDrawIn(_attachment);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, m_fboId);

            //_fboWrapperBlit.
            GL.BlitFramebuffer(0, 0, SIZE, SIZE, 0, 0, SIZE, SIZE, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

            ////glBindFramebuffer(GL_FRAMEBUFFER,m_frameBuffer);
            //glBindBuffer(GL_PIXEL_PACK_BUFFER,m_subImageBuffer[i]);
            //glPixelStorei(GL_PACK_ALIGNMENT,1);

            //glBindFramebuffer(GL_FRAMEBUFFER,m_frameBufferBlit);
            ////注意：以BGR的顺序读取
            //glReadPixels(0,0,m_subImageWidth,m_subImageHeight,
            //    GL_BGR,GL_UNSIGNED_BYTE,bufferOffset(0));
            //int temp4 = GLUtils::checkForOpenGLError(__FILE__,__LINE__);
            //m_subPixels[i] = static_cast<GLubyte*>(glMapBuffer(GL_PIXEL_PACK_BUFFER,GL_READ_ONLY));
            //if (m_subPixels[i] == NULL)
            //{
            //    cout << "NULL pointer" << endl;
            //}
            //gltGenBMP(subImageFile[i],GLT_BGR,m_subImageWidth,m_subImageHeight,m_subPixels[i]);
            //glUnmapBuffer(GL_PIXEL_PACK_BUFFER);
            //glBindBuffer(GL_PIXEL_PACK_BUFFER,0);

            //glBindFramebuffer(GL_FRAMEBUFFER,0);
        }
    }
}