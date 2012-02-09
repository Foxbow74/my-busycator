using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Objects.Furniture;

namespace GameCore.Acts.Movement
{
	internal class Ascend : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 300; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.OemComma, EKeyModifiers.SHIFT); }
		}

		public override string Name
		{
			get { return "подняться по лестнице"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.MOVEMENT; }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var furniture = _creature.MapCell.Furniture;
			if (!(furniture is StairUp))
			{
				if (!_silence)
				{
					MessageManager.SendMessage(this, "куда? Тут нет лестницы");
				}
				return EActResults.QUICK_FAIL;
			}
			return ((Stair) furniture).MoveToLayer(_creature);
		}
	}
}