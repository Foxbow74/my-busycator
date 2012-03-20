using System;
using System.IO;
using System.Reflection;
using GameCore;
using GameCore.Misc;
using GameUi;
using OpenTK;
using OpenTKUi;
using Point = GameCore.Misc.Point;

namespace Busycator
{
	internal class GameProvider : OpenTKGameProvider
	{
		private readonly TheGame m_game;

		public GameProvider() : base(16, 16, 700, 500)
		{
			Title = "Busycator";
			m_game = new TheGame(this);
		}

		protected override void MouseButtonDown(Point _pnt, EMouseButton _button)
		{
			m_game.MouseButtonDown(_pnt, _button);
		}

		protected override void MouseButtonUp(Point _pnt, EMouseButton _button)
		{
			m_game.MouseButtonUp(_pnt, _button);
		}

		protected override void MouseMove(Point _pnt)
		{
			m_game.MouseMove(_pnt);
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
			if (IsActive)
			{
				m_game.WindowClientSizeChanged(WidthInCells, HeightInCells);
			}
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

			var avatar = World.TheWorld.Avatar;
			Title = "Busycator lc:" + avatar[0,0].LiveCoords + " wc:" + avatar[0,0].WorldCoords + " bld:" + avatar[0,0].InBuilding;
			m_game.Update(KeyState);
		}

		protected override void OnRenderFrame(FrameEventArgs _e)
		{
			if (!IsActive) return;

			//if (m_game.IsNeedDraw)
			{
				Clear(FColor.Empty);

				using (new Profiler("m_game.Draw();"))
				{
					m_game.Draw();
				}

				OnRenderFinished();
			}
		}

		[STAThread]
		private static void Main()
		{
			{
				try
				{
					using (var game = new GameProvider() { Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location) })
					{
						game.Run(0,0);
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