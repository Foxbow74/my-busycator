using System;
using GameUi;
using OpenTKUi;

namespace Busycator
{
	class GameProvider:OpenTKGameProvider
	{
		private readonly TheGame m_game;

		public GameProvider(): base(16, 16, 800, 600)
		{
			m_game = new TheGame(this);
		}

		protected override void OnLoad(EventArgs _e)
		{
			base.OnLoad(_e);
			m_game.LoadContent(ResourceProvider);
		}

		protected override void OnResize(EventArgs _e)
		{
			base.OnResize(_e);
			m_game.WindowClientSizeChanged(WidthInCells, HeightInCells);
		}

		protected override void OnUnload(EventArgs _e)
		{
			m_game.UnloadContent();
			base.OnUnload(_e);
		}

		protected override void OnUpdateFrame(OpenTK.FrameEventArgs _e)
		{
			base.OnUpdateFrame(_e);
			m_game.Update(KeyState);
		}

		protected override void OnRenderFrame(OpenTK.FrameEventArgs _e)
		{
			Title = "Busycator FPS: " + (1 / _e.Time).ToString("0.");
			//Clear(Color.Black);
			m_game.Draw();
			SwapBuffers();
		}

		[STAThread]
		static void Main()
		{
			using (var game = new GameProvider())
			{
				game.Run(0.0, 0.0);
			}
		}
	}
}
