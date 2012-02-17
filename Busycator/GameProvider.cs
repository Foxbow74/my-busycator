using System;
using System.IO;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK;
using OpenTKUi;

namespace Busycator
{
	internal class GameProvider : OpenTKGameProvider
	{
		private readonly TheGame m_game;

		public GameProvider() : base(16, 16, 700, 500)
		{
			m_game = new TheGame(this);
		}

		protected override void OnLoad(EventArgs _e)
		{
			base.OnLoad(_e);
			m_game.LoadContent(ResourceProvider);
			OnLoadFinished();
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
			Profiler.Report();
		}

		protected override void OnUpdateFrame(FrameEventArgs _e)
		{
			base.OnUpdateFrame(_e);
			if (!IsActive) return;

			m_game.Update(KeyState);
		}

		protected override void OnRenderFrame(FrameEventArgs _e)
		{
			if (!IsActive) return;
			Title = "Busycator FPS:" + (1 / _e.Time).ToString("0.") + " Avatar:" + World.TheWorld.Avatar.Coords;
			Clear(FColor.Empty);
			m_game.Draw();
			OnRenderFinished();
		}

		[STAThread]
		private static void Main()
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