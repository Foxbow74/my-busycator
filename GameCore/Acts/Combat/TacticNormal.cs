using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Messages;

namespace GameCore.Acts.Combat
{
	class TacticNormal : Act
	{
		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.F2, EKeyModifiers.NONE); }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EALConst Name
		{
			get { return EALConst.AN_TACTIC_NORMAL; }
		}

		protected override int TakeTicksOnSingleAction
		{
			get { return 1; }
		}

		public override EActResults Do(Creature _creature)
		{
			if (_creature.IsAvatar)
			{
				World.TheWorld.Avatar.Tactic = ETactics.NORMAL;
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "В бою " + Name));
			}
			return EActResults.DONE;
		}
	}
}