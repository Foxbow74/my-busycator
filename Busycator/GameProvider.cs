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

			Title = "Busycator FPS:" + (1 / _e.Time).ToString("0.") + " lc:" + World.TheWorld.Avatar[0,0].LiveCoords + " wc:" + World.TheWorld.Avatar[0,0].WorldCoords;
			using (new Profiler())
			{
				m_game.Update(KeyState);
			}
		}

		protected override void OnRenderFrame(FrameEventArgs _e)
		{
			if (!IsActive) return;

			//if (m_game.IsNeedDraw)
			{
				Clear(FColor.Empty);
				m_game.Draw();

				OnRenderFinished();
			}
		}

		[STAThread]
		private static void Main()
		{
			{
				//Func<int, float> f = _i => (float)Math.Round(_i/255.0, 2);
				//foreach (var variable in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public))
				//{
				//    var color = (Color)variable.GetValue(null, new object[0]);
				//    Debug.WriteLine(string.Format("public static readonly FColor {0} = new FColor({1}f, {2}f, {3}f, {4}f);", variable.Name, f(color.A), f(color.R), f(color.G), f(color.B)));
				//}
				try
				{
					using (var game = new GameProvider())
					{
						game.Run(60.0,60.0);
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