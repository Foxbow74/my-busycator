using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Shader
{
    class ShaderWrapper
    {
        private readonly int m_shaderProgramHandle;

        public ShaderWrapper(string _text)
        {
            var shaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(shaderHandle, _text);
            GL.CompileShader(shaderHandle);
            m_shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(m_shaderProgramHandle, shaderHandle);
            GL.LinkProgram(m_shaderProgramHandle);
            var programInfoLog = GL.GetProgramInfoLog(m_shaderProgramHandle);
            if (!string.IsNullOrEmpty(programInfoLog))
            {
                throw new ApplicationException(string.Format("Bad shader:{0}{1}{0}{0}{2}", Environment.NewLine, programInfoLog, _text));
            }
        }

        public class DrawHelper : IDisposable
        {
            private readonly ShaderWrapper m_wrapper;

            public DrawHelper(ShaderWrapper _wrapper)
            {
                m_wrapper = _wrapper;
                GL.UseProgram(_wrapper.m_shaderProgramHandle);
            }

            public void BindTexture(int _textureId, TextureUnit _textureUnit, string _uniformName)
            {
                GL.ActiveTexture(_textureUnit);
                GL.BindTexture(TextureTarget.Texture2D, _textureId);
                GL.Uniform1(GL.GetUniformLocation(m_wrapper.m_shaderProgramHandle, _uniformName), _textureUnit - TextureUnit.Texture0);
            }


            public void BindVec2(PointF _pnt, string _uniformName)
            {
                GL.Uniform2(GL.GetUniformLocation(m_wrapper.m_shaderProgramHandle, _uniformName), 1, new[] { _pnt.X, _pnt.Y });
            }

            public void Dispose()
            {
                GL.UseProgram(0);
            }
        }
    }
}
