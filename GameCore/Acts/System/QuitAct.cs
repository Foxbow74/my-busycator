using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Acts.System
{
	internal class QuitAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
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

		public override EActionCategory Category
		{
			get { return EActionCategory.SYSTEM; }
		}

		public override EActResults Do(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}
}