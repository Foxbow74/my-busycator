using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Acts
{
	public class WaitAct:Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 100; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get
			{
				yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.NumPad5, EKeyModifiers.NONE);
			}
		}

		public override string Name
		{
			get { return "ждать"; }
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
			return EActResults.DONE;
		}
	}
}