using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using LanguagePack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
    public abstract class AbstractGameTestX : AbstractTest
	{
		static AbstractGameTestX()
		{
			Constants.GAME_MODE = false;
		}

		public virtual void SendKey(ConsoleKey _key, EKeyModifiers _modifiers = EKeyModifiers.NONE)
		{
			World.TheWorld.KeyPressed(_key, _modifiers);
			do
			{
				World.TheWorld.GameUpdated();
			} while (World.TheWorld.CreatureManager.FirstActiveCreature != Avatar);
		}

		public Avatar Avatar
		{
			get { return World.TheWorld.Avatar; }
		}

		private void MessageManagerOnNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.TURN:
					World.TheWorld.UpdateDPoint();
					break;
				case WorldMessage.EType.AVATAR_BEGINS_TURN:
					AvatarBeginsTurn();
					break;
				case WorldMessage.EType.AVATAR_MOVE:
					break;
				case WorldMessage.EType.AVATAR_CHANGE_LAYER:
					break;
				case WorldMessage.EType.JUST_REDRAW:
					break;
				case WorldMessage.EType.MICRO_TURN:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			//Debug.WriteLine("WorldMessage : " + _message.Type);
		}


		private readonly List<XLangMessage> m_messages = new List<XLangMessage>();

		private void MessageManagerOnNewMessage(object _sender, Message _message)
		{
			if (_message is XLangMessage)
			{
				m_messages.Add((XLangMessage)_message);
			}
			else
			{
				Debug.WriteLine(_message.ToString());
			}
		}

		protected virtual void AvatarBeginsTurn()
		{
			if (m_messages.Count > 0)
			{
				var enumerable = XMessageCompiler.Compile(m_messages).ToArray();
				Debug.WriteLine(string.Join(", ", enumerable));
				m_messages.Clear();
			}
		}
	
		[TestInitialize]
		public void Initialize()
		{
			MagickSetting.MagicSettingProvider.Init();

			foreach (ETileset tileset in Enum.GetValues(typeof(ETileset)))
			{
				TileSetInfoProvider.SetOpacity(tileset, 0, 0f);
			}

			if (World.TheWorld == null)
			{
				World.LetItBeeee(new RusLanguageProcessor());
			}
			MessageManager.NewWorldMessage += MessageManagerOnNewWorldMessage;

			World.TheWorld.LiveMap.SetViewPortSize(new Point(100, 100));

			MessageManager.NewMessage += MessageManagerOnNewMessage;
		}

		[TestCleanup]
		public void Cleanup()
		{
			Profiler.Report();
			MessageManager.NewMessage -= MessageManagerOnNewMessage;
			MessageManager.NewWorldMessage -= MessageManagerOnNewWorldMessage;
			GC.Collect();
			GC.Collect();
			GC.WaitForFullGCComplete();
		}
	}
}