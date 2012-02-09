using System;
using System.IO;
using GameUi;
using OpenTKUi;

namespace Busycator
{
	class GameProvider:OpenTKGameProvider
	{
		private readonly TheGame m_game;

		public GameProvider(): base(16, 16, 1024, 768)
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
			m_game.Draw();
			SwapBuffers();
		}

		[STAThread]
		static void Main()
		{
			{
				try
				{
					using (var game = new GameProvider())
					{
						game.Run(0.0, 0.0);
					}
				}
				catch (Exception exception)
				{
					var path = Path.Combine(Environment.CurrentDirectory, "error_file.txt");
					File.Delete(path);
					File.AppendAllText(path, exception.Message);
					File.AppendAllText(path, exception.StackTrace);
				}
			}
		}
	}
}
