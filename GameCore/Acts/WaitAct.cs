using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences;

namespace GameCore.Acts
{
	public class WaitAct : Act, ISpecial
	{
		protected override int TakeTicksOnSingleAction { get { return 100; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get
			{
				yield break;
				//yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.NumPad5, EKeyModifiers.NONE);
			}
		}

		public override EALConst Name { get { return EALConst.AN_WAIT; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.SYSTEM; } }

		public override EActResults Do(Creature _creature) { return EActResults.WORLD_STAYS_UNCHANGED; }
	}
}