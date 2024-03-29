﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using GameCore;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Shader
{
    internal class FboWrapper : IDisposable
    {
        public const int SIZE = 128;
        private int m_fboId;
        private readonly List<ColorBuffer> m_buffers = new List<ColorBuffer>();
        private int m_renderBuffer;
        private const int SAMPLES = 4;

        public FboWrapper(bool _addRenderBuffer)
        {
            GL.Ext.GenFramebuffers(1, out m_fboId);

            if (_addRenderBuffer)
            {
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboId);
                GL.Ext.GenRenderbuffers(1, out m_renderBuffer);
                GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, m_renderBuffer);
                GL.Ext.RenderbufferStorageMultisample(
                    (ExtFramebufferMultisample) (int) RenderbufferTarget.RenderbufferExt, SAMPLES,
                    (ExtFramebufferMultisample) (int) PixelFormat.Rgba, SIZE, SIZE);
                GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0,
                    RenderbufferTarget.RenderbufferExt, m_renderBuffer);
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            }

            Check();
        }

        public int this[int _index]
        {
            get
            {
                return m_buffers[_index].TextureId;
            }
        }

        public int AddTextureBuffer()
        {
            var t = new ColorBuffer();
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, m_fboId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                FramebufferAttachment.ColorAttachment0 + m_buffers.Count, TextureTarget.Texture2D, t.TextureId, 0);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

            m_buffers.Add(t);
            Check();
            return t.TextureId;
        }

        public int CountOfBuffers
        {
            get
            {
                return m_buffers.Count;
            }
        }

        public void Check()
        {
            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                {
                    break;
                }
                case FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                {
                    Console.WriteLine(
                        "FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                    break;
                }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                {
                    Console.WriteLine("FBO: There are no attachments.");
                    break;
                }
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                {
                    Console.WriteLine(
                        "FBO: Attachments are of different size. All attachments must have the same width and height.");
                    break;
                }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                {
                    Console.WriteLine(
                        "FBO: The color attachments have different format. All color attachments must have the same format.");
                    break;
                }
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                {
                    Console.WriteLine(
                        "FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                    break;
                }
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                {
                    Console.WriteLine(
                        "FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
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
            private readonly int[] m_savedViewport = new int[4];

            public DrawHelper(FboWrapper _fboWrapper)
            {
                var fboWrapper = _fboWrapper;

                GL.Flush();
                GL.GetInteger(GetPName.Viewport, m_savedViewport);
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fboWrapper.m_fboId);

                GL.PushAttrib(AttribMask.ViewportBit);
                GL.Viewport(0, 0, SIZE, SIZE);

                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.LoadIdentity();
                GL.Ortho(0, SIZE, SIZE, 0, -1, 1);
                GL.Scale(1f, -1f, 1f);
                GL.Translate(0f, -SIZE, 0f);

                GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, fboWrapper.m_renderBuffer);
                GL.BindTexture(TextureTarget.Texture2D, 0);

                //GL.Enable(EnableCap.Blend);
                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            }

            public static void Screenshot(string _filename)
            {
                var arr = new FColor[LosManagerEx.MAP_SIZE, LosManagerEx.MAP_SIZE];
                GL.ReadPixels(0, 0, LosManagerEx.MAP_SIZE, LosManagerEx.MAP_SIZE, PixelFormat.Rgba, PixelType.Float, arr);

                var bmp = new Bitmap(LosManagerEx.MAP_SIZE, LosManagerEx.MAP_SIZE,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                for (var i = 0; i < LosManagerEx.MAP_SIZE; i++)
                {
                    for (var j = 0; j < LosManagerEx.MAP_SIZE; j++)
                    {
                        bmp.SetPixel(j, i,
                            Color.FromArgb((int) (arr[i, j].A*255), (int) (arr[i, j].R*255), (int) (arr[i, j].G*255),
                                (int) (arr[i, j].B*255)));
                    }
                }
                bmp.Save(_filename, ImageFormat.Png);
            }

            public void Dispose()
            {
                GL.Flush();
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.PopAttrib();

                GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, 0);
                GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                GL.Viewport(m_savedViewport[0], m_savedViewport[1], m_savedViewport[2], m_savedViewport[3]);
            }
        }

        private class ColorBuffer
        {
            private readonly int m_textureId;

            public ColorBuffer()
            {
                GL.GenTextures(1, out m_textureId);
                GL.BindTexture(TextureTarget.Texture2D, TextureId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int) TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Clamp);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, SIZE, SIZE, 0, PixelFormat.Rgb,
                    PixelType.UnsignedByte, IntPtr.Zero);
                GL.BindTexture(TextureTarget.Texture2D, 0);
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
            GL.Ext.DeleteRenderbuffers(1, ref m_renderBuffer);
            GL.Ext.DeleteFramebuffers(1, ref m_fboId);
        }

        public void BlitTo(FboWrapper _fboWrapperBlit, int _attachment)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _fboWrapperBlit.m_fboId);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, m_fboId);

            GL.DrawBuffer((DrawBufferMode) FramebufferAttachment.ColorAttachment0Ext + _attachment);
            GL.BindTexture(TextureTarget.Texture2D, _fboWrapperBlit.m_buffers[_attachment].TextureId);

            GL.BlitFramebuffer(0, 0, SIZE, SIZE, 0, 0, SIZE, SIZE, ClearBufferMask.ColorBufferBit,
                BlitFramebufferFilter.Linear);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
        }

        public void BindTexture(int _attachment)
        {
            GL.BindTexture(TextureTarget.Texture2D, m_buffers[_attachment].TextureId);
        }
    }
}