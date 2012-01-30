using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Acts
{
	class InventoryAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.I, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "показать инвентарь"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}
	}

	class QuitAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Q, EKeyModifiers.CTRL); }
		}

		public override string Name
		{
			get { return "выйти из игры"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}
	}

	class HelpAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Oem2, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "показать экран помощи"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}
	}
}
