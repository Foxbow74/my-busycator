using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Messages;

namespace GameCore.Acts.Combat
{
	class TacticBerserk : Act
	{
		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.F3, EKeyModifiers.NONE); }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EALConst Name
		{
			get { return EALConst.AN_TACTIC_BERSERK; }
		}

		protected override int TakeTicksOnSingleAction
		{
			get { return 1; }
		}

		public override EActResults Do(Creature _creature)
		{
			if (_creature.IsAvatar)
			{
				World.TheWorld.Avatar.Tactic = ETactics.BERSERK;
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Текущий " + Name));
			}
			return EActResults.DONE;
		}
	}

	class TacticPeacefull : Act
	{
		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.F4, EKeyModifiers.NONE); }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EALConst Name
		{
			get { return EALConst.AN_TACTIC_PEACEFULL; }
		}

		protected override int TakeTicksOnSingleAction
		{
			get { return 1; }
		}

		public override EActResults Do(Creature _creature)
		{
			if (_creature.IsAvatar)
			{
				World.TheWorld.Avatar.Tactic = ETactics.PEACEFULL;
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Текущий " + Name));
			}
			return EActResults.DONE;
		}
	}
}