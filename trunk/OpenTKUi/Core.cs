using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace OpenTKUi
{
	public class Core
	{
		public void Init()
		{
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
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}


		/// <summary>
		/// 	Resizes viewport.
		/// </summary>
		/// <param name = "_windowW">New width of window.</param>
		/// <param name = "_windowH">New height of window.</param>
		public void Resize(int _windowW, int _windowH)
		{
			GL.Viewport(new Size(_windowW, _windowH));
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();

			var viewPort = new int[4];
			GL.GetInteger(GetPName.Viewport, viewPort);
			GL.Ortho(viewPort[0], viewPort[0] + viewPort[2], viewPort[1] + viewPort[3], viewPort[1], -1, 1);
		}


		/// <summary>
		/// 	Sets color to be cleared on Clear() call.
		/// </summary>
		public void SetClearColor(float _r, float _g, float _b)
		{
			GL.ClearColor(_r, _g, _b, 0.0f);
		}


		/// <summary>
		/// 	Clears the background.
		/// </summary>
		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		/// <summary>
		/// 	Sets drawing color.
		/// </summary>
		/// <param name = "_r">Red intensity.</param>
		/// <param name = "_g">Green intensity.</param>
		/// <param name = "_b">Blue intensity.</param>
		/// <param name = "_a">Alpha intensity.</param>
		public void Color(byte _r, byte _g, byte _b, byte _a)
		{
			GL.Color4(_r, _g, _b, _a);
		}

		/// <summary>
		/// 	Resets color to white and loads identity.
		/// </summary>
		public void Reset()
		{
			GL.Color4((byte) 255, (byte) 255, (byte) 255, (byte) 255);
			GL.LoadIdentity();
			GL.Translate(0.375, 0.375, 0.0); // Move slightly for pixel precise drawing
		}
	}
}