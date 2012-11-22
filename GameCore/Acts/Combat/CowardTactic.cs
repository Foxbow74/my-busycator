using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Acts.Combat
{
	class TacticCoward : Act
	{
		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.F1, EKeyModifiers.NONE); }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { return "приоритет в бою - защита"; }
		}

		protected override int TakeTicksOnSingleAction
		{
			get { return 1; }
		}

		public override EActResults Do(Creature _creature)
		{
			if (_creature.IsAvatar)
			{
				World.TheWorld.Avatar.Tactic = ETactics.COWARD;
			}
			return EActResults.DONE;
		}
	}

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

		public override string Name
		{
			get { return "приоритет в бою - атака"; }
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
			}
			return EActResults.DONE;
		}
	}

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

		public override string Name
		{
			get { return "соблюдать в бою разумный баланс атаки и защиты"; }
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
			}
			return EActResults.DONE;
		}
	}
}
