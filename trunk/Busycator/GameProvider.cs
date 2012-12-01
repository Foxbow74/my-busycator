using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameUi;
using MagickSetting;
using OpenTK;
using OpenTKUi;
using UnsafeUtils;

namespace Busycator
{
	internal class GameProvider : OpenTKGameProvider
	{
		const int FPS = 60;
		private readonly TheGame m_game;
		private readonly HiResTimer m_hrs = new HiResTimer();

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
				MessageManager.SendMessage(this,WorldMessage.JustRedraw);
			}
		}

		protected override void OnUnload(EventArgs _e)
		{
			m_game.UnloadContent();
			base.OnUnload(_e);
			Profiler.Report();
		}

		private Int64 sum = 16;
		private Int64 cnt = 1;

		protected override void OnRenderFrame(FrameEventArgs _e)
		{
			var now = m_hrs.Value;
			if (IsActive)
			{

				var avatar = World.TheWorld.Avatar;
				Title = string.Format("Busycator lc:{0} wc:{1} bld:{2} pmc:{3} fps:{4} per frame, ms:{5}", avatar[0, 0].LiveCoords, avatar[0, 0].WorldCoords, avatar[0, 0].InBuilding, avatar[0, 0].PathMapCoords, Math.Round(1 / _e.Time), (sum/cnt));

				//base.OnRenderFrame(_e);

				m_game.Update(KeyState);

				using (new Profiler("Redraw"))
				{
					Clear(FColor.Empty);
					m_game.Draw();
					OnRenderFinished();
				}
			}
			var milliseconds = (int)m_hrs.GetMilliseconds(now);
			sum += milliseconds;
			cnt++;
			if (milliseconds < 1000 / FPS)
			{
				Thread.Sleep(1000 / FPS - milliseconds);
			}

		}

		[STAThread] private static void Main()
		{
			{
				MagicSettingProvider.Init();
				try
				{
					using (var game = new GameProvider {Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location)})
					{
						game.Run(0, FPS);
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