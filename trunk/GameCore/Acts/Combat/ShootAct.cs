using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Messages;

namespace GameCore.Acts.Combat
{
	class ShootAct:Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 30; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.F, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "выстрелить"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			MessageManager.SendMessage(this, new AskShootTargerMessage(this, 10));
			return EActResults.DONE;
		}
	}
}
