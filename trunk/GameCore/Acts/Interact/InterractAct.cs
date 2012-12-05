using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences.Mechanisms;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Acts.Interact
{
	public class InterractAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 200; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Y, EKeyModifiers.NONE); }
		}

		public override EALConst Name
		{
			get { return EALConst.AN_INTERRACT_WITH_ESSENCE; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.WORLD_INTERACTIONS; }
		}

		public override EActResults Do(Creature _creature)
		{
			LiveMapCell liveMapCell;
			var find = Find(_creature, (_essence, _cell) => _essence is IInteractiveThing, out liveMapCell);
			switch (find)
			{
				case EActResults.QUICK_FAIL:
					if (_creature.IsAvatar)MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "взаимодействовать с чем?"));
					return find;
				case EActResults.NONE:
					break;
				default:
					return find;
			}

			if (liveMapCell != null)
			{
				return ((IInteractiveThing)liveMapCell.Thing).Interract(_creature, liveMapCell);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}