using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Essences.Things;

namespace GameCore.Acts.Movement
{
	internal class Ascend : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 300; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.OemComma, EKeyModifiers.SHIFT); } }

		public override string Name { get { return "��������� �� ��������"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.MOVEMENT; } }

		public override EActResults Do(Creature _creature)
		{
			var thing = _creature[0, 0].Thing;
			if (!(thing is StairUp))
			{
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, "����? ��� ��� ��������");
				}
				return EActResults.QUICK_FAIL;
			}
			return ((Stair) thing).MoveToLayer(_creature);
		}
	}
}