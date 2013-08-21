using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences.Things;
using GameCore.Messages;

namespace GameCore.Acts.Movement
{
	internal class Ascend : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 300; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.OemComma, EKeyModifiers.SHIFT); } }

		public override EALConst Name { get { return EALConst.AN_ASCEND; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.MOVEMENT; } }

		public override EActResults Do(Creature _creature)
		{
			var thing = _creature[0, 0].GetResolvedThing(_creature);
			if (!(thing is StairUp))
			{
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, "куда? Тут нет лестницы");
				}
				return EActResults.QUICK_FAIL;
			}
			return ((Stair) thing).MoveToLayer(_creature);
		}
	}
}