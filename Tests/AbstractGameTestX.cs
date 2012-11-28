using System;
using GameCore;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public abstract class AbstractGameTestX
	{
		public virtual void SendKey(ConsoleKey _key, EKeyModifiers _modifiers = EKeyModifiers.NONE)
		{
			World.TheWorld.KeyPressed(_key, _modifiers);
			do
			{
				World.TheWorld.GameUpdated();
			} while (World.TheWorld.LiveMap.FirstActiveCreature != Avatar);
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

		protected virtual void AvatarBeginsTurn()
		{
		}

		static void MessageManagerNewMessage(object _sender, Message _message)
		{
			//Debug.WriteLine("Message : " + _message.GetType() + " / " + _message);
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
				World.LetItBeeee();
			}
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerOnNewWorldMessage;

			World.TheWorld.LiveMap.SetViewPortSize(new Point(100, 100));
		}

		[TestCleanup]
		public void Cleanup()
		{
			Profiler.Report();
			MessageManager.NewMessage -= MessageManagerNewMessage;
			MessageManager.NewWorldMessage -= MessageManagerOnNewWorldMessage;
		}
	}
}