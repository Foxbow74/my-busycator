using System;
using System.Diagnostics;
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
		public void SendKey(ConsoleKey _key, EKeyModifiers _modifiers = EKeyModifiers.NONE)
		{
			World.TheWorld.KeyPressed(_key, _modifiers);
			World.TheWorld.GameUpdated();
		}

		public Avatar Avatar
		{
			get { return World.TheWorld.Avatar; }
		}

		private static void MessageManagerOnNewWorldMessage(object _sender, WorldMessage _message)
		{
			Debug.WriteLine("WorldMessage : " + _message.Type);
		}

		static void MessageManagerNewMessage(object _sender, Message _message)
		{
			Debug.WriteLine("Message : " + _message.GetType() + " / " + _message);
		}

		[TestInitialize]
		public void Initialize()
		{
			using (new Profiler())
			{
				if (World.TheWorld == null)
				{
					World.LetItBeeee();
				}
			}
			MessageManager.NewMessage += MessageManagerNewMessage;
			MessageManager.NewWorldMessage += MessageManagerOnNewWorldMessage;
		}

		[TestCleanup]
		public void Cleanup()
		{
			Profiler.Report();
			MessageManager.NewMessage -= MessageManagerNewMessage;
			MessageManager.NewWorldMessage -= MessageManagerOnNewWorldMessage;
		}
	}

	[TestClass]
	public abstract class AbstractGameTest0 : AbstractGameTestX
	{
		static AbstractGameTest0()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 0;
		}
	}

	[TestClass]
	public abstract class AbstractGameTest1 : AbstractGameTestX
	{
		static AbstractGameTest1()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 1;
		}
	}

	[TestClass]
	public abstract class AbstractGameTest2 : AbstractGameTestX
	{
		static AbstractGameTest2()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 2;
		}
	}
}