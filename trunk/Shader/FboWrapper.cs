using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Shader
{
    class FboWrapper:IDisposable
    {
        public const int SIZE = 512;
        private int m_fboId;
        private readonly List<ColorBuffer> m_buffers=new List<ColorBuffer>();

        public FboWrapper()
        {
            // Create a FBO and attach the textures
            GL.Ext.GenFramebuffers(1, out m_fboId);
        }

        public int AddTextureBuffer()
        {
            var t = new ColorBuffer();
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

                m_fboWrapper.Check();
                GL.PushMatrix();
                GL.Scale(1f, -1f, 1f);
                GL.Translate(0f, -512f, 0f);

            }

            public void BeginDrawIn(int _i)
            {
                GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0 + _i);
                GL.BindTexture(TextureTarget.Texture2D, m_fboWrapper.m_buffers[_i].TextureId);
            }

            public void Dispose()
            {
                GL.Flush();
                //GL.Scale(1, 1, 1);
                GL.PopMatrix();
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
            
            GL.Ext.DeleteFramebuffers(1, ref m_fboId);
        }
    }
}