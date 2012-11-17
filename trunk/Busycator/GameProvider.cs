using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using GameCore;
using GameCore.Creatures;
using GameCore.Misc;
using GameUi;
using OpenTK;
using OpenTKUi;

namespace Busycator
{
	internal class GameProvider : OpenTKGameProvider
	{
		private readonly TheGame m_game;

		public GameProvider()
			: base(1024, 768)
		{
			Title = "Busycator";
			m_game = new TheGame(this);
		}

		protected override void MouseButtonDown(Point _pnt, EMouseButton _button) { m_game.MouseButtonDown(_pnt, _button); }

		protected override void MouseButtonUp(Point _pnt, EMouseButton _button) { m_game.MouseButtonUp(_pnt, _button); }

		protected override void MouseMove(Point _pnt) { m_game.MouseMove(_pnt); }

		protected override void OnLoad(EventArgs _e)
		{
			base.OnLoad(_e);
			m_game.LoadContent(ResourceProvider);
			OnLoadFinished();
		}

		protected override void OnResize(EventArgs _e)
		{
			base.OnResize(_e);
			if (IsActive)
			{
				m_game.WindowClientSizeChanged(Width / Constants.TILE_SIZE, Height / Constants.TILE_SIZE);
			}
		}

		protected override void OnUnload(EventArgs _e)
		{
			m_game.UnloadContent();
			base.OnUnload(_e);
			Profiler.Report();
		}

		protected override void OnRenderFrame(FrameEventArgs _e)
		{
			DateTime now = DateTime.Now;
			if (IsActive)
			{

				var avatar = World.TheWorld.Avatar;
				Title = "Busycator lc:" + avatar[0, 0].LiveCoords + " wc:" + avatar[0, 0].WorldCoords + " bld:" + avatar[0, 0].InBuilding + " pmc:" + avatar[0, 0].PathMapCoords + " fps:" + Math.Round(1 / _e.Time);

				base.OnRenderFrame(_e);
	
				m_game.Update(KeyState);
				
				//if (m_game.IsNeedDraw)
				{
					Clear(FColor.Empty);
					m_game.Draw();
					OnRenderFinished();
				}
			}
			var ts = DateTime.Now - now;
			if (ts.Milliseconds < 100 / 6)
			{
				Thread.Sleep(100 / 6 - ts.Milliseconds);
			}

		}

		[STAThread] private static void Main()
		{
			{
				try
				{
					using (var game = new GameProvider {Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location)})
					{
						game.Run(60);
					}
				}
				catch (Exception exception)
				{
					var path = Path.Combine(Environment.CurrentDirectory, "error_file.txt");
					File.Delete(path);
					File.AppendAllText(path, exception.Message);
					File.AppendAllText(path, exception.StackTrace);
					Process.Start("error_file.txt");
				}
			}
		}
	}
}