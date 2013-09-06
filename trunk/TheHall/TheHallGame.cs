using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameUi;
using LanguagePack;
using MagickSetting;
using OpenTK;
using OpenTKUi;
using Shader;
using TheHall.Layers;

namespace TheHall
{
    public class TheHallGame : OpenTKGameProvider
    {
        const int FPS = 60;
        private readonly TheGame m_game;
        private readonly Stopwatch m_stopwatch = new Stopwatch();

        public TheHallGame()
            : base(200, 200)
        {
            World.SetStartingLayerType<TheHallLayer>();
            Title = "The Hall";
            m_game = new TheGame(this);
        }

        protected override void MouseButtonDown(Point _pnt, EMouseButton _button) { m_game.MouseButtonDown(_pnt, _button); }

        protected override void MouseButtonUp(Point _pnt, EMouseButton _button) { m_game.MouseButtonUp(_pnt, _button); }

        protected override void MouseMove(Point _pnt) { m_game.MouseMove(_pnt); }

        public override IEnumerable<IAbstractLanguageProcessor> GetLanguageProcessors()
        {
            yield return new RusLanguageProcessor();
        }

        protected override void OnLoad(EventArgs _e)
        {
            base.OnLoad(_e);
            m_game.LoadContent(ResourceProvider);

            MessageManager.NewWorldMessage += MessageManagerOnNewWorldMessage;
            OnLoadFinished();
            m_game.Run();
        }

        private void MessageManagerOnNewWorldMessage(object _sender, WorldMessage _message)
        {
            if (_message.Type == WorldMessage.EType.TURN || _message.Type == WorldMessage.EType.JUST_REDRAW)
            {
                m_needRedraw = true;
            }
        }

        protected override void OnResize(EventArgs _e)
        {
            base.OnResize(_e);
            if (IsActive)
            {
                m_game.WindowClientSizeChanged(Width / Constants.TILE_SIZE, Height / Constants.TILE_SIZE);
                MessageManager.SendMessage(this, WorldMessage.JustRedraw);
            }
        }

        protected override void OnUnload(EventArgs _e)
        {
            m_game.UnloadContent();
            base.OnUnload(_e);
            Profiler.Report();
        }

        private Int64 m_sum = 16;
        private Int64 m_cnt = 1;
        private bool m_needRedraw;

        protected override void OnRenderFrame(FrameEventArgs _e)
        {
            m_stopwatch.Restart();
            if (IsActive)
            {
                if (World.TheWorld != null)
                {
                    var avatar = World.TheWorld.Avatar;
                    Title = string.Format("The Hall lc:{0} wc:{1} bld:{2} pmc:{3} fps:{4} per frame, ms:{5}, creatures:{6}",
                                          avatar[0, 0].LiveCoords, avatar[0, 0].WorldCoords, avatar[0, 0].InBuilding,
                                          avatar[0, 0].PathMapCoords, Math.Round(1 / _e.Time), (m_sum / m_cnt),
                                          World.TheWorld.CreatureManager.InfoByCreature.Count);
                }

                m_game.Update(KeyState);
                if (m_needRedraw)
                {
                    m_needRedraw = false;
                    Clear(FColor.Empty);
                    m_game.Draw();
                    OnRenderFinished();
                }
            }
            m_stopwatch.Stop();
            m_sum += m_stopwatch.ElapsedMilliseconds;
            m_cnt++;
            if (m_stopwatch.ElapsedMilliseconds < 1000 / FPS)
            {
                Thread.Sleep(1000 / FPS - (int)m_stopwatch.ElapsedMilliseconds);
            }

        }

        [STAThread]
        public static void Main()
        {
            {
                MagicSettingProvider.Init();
                try
                {
                    using (var game = new TheHallGame { Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location) })
                    {
                        Constants.WORLD_MAP_SIZE = 1;
                        Constants.WORLD_SEED = new Random().Next(10000);
                        game.Run(0, FPS);
                    }
                }
                catch (Exception exception)
                {
                    var path = Path.Combine(Environment.CurrentDirectory, "error_file.txt");
                    File.Delete(path);
                    File.AppendAllText(path, exception.Message, Encoding.Unicode);
                    File.AppendAllText(path, exception.StackTrace, Encoding.Unicode);
                    Process.Start("error_file.txt");
                }
            }
        }
    }
}
