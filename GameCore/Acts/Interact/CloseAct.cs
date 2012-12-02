using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Acts.Interact
{
	public class CloseAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 20; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.C, EKeyModifiers.NONE); } }

		public override string Name { get { return "Закрыть сундук/дверь"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.WORLD_INTERACTIONS; } }

		public override EActResults Do(Creature _creature)
		{
			LiveMapCell liveMapCell;

			var find = Find(_creature, (_essence, _cell) => _essence.CanBeClosed(_cell, _creature), out liveMapCell);
			switch (find)
			{
				case EActResults.QUICK_FAIL:
					if (_creature.IsAvatar) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));
					return find;
				case EActResults.NONE:
					break;
				default:
					return find;
			}

			//выясняем, что нужно закрыть
			{
				var list = new List<EssenceDescriptor>();
				if (liveMapCell.Thing.Is<ICanbeClosed>())
				{
					list.Add(new EssenceDescriptor(liveMapCell.Thing, liveMapCell.LiveCoords, null));
				}
				list.AddRange(liveMapCell.GetAllAvailableItemDescriptors<Thing>(_creature).Where(_descriptor => _descriptor.Essence.CanBeClosed(liveMapCell, _creature)));
				if (liveMapCell.LiveCoords == _creature.GeoInfo.LiveCoords)
				{
					list.AddRange(_creature.GetBackPackItems().Where(_descriptor => _descriptor.Essence.CanBeClosed(liveMapCell, _creature)));
				}
				var descriptors = list.Distinct();
				if (GetParameter<EssenceDescriptor>().Any())
				{
					descriptors = GetParameter<EssenceDescriptor>().Intersect(descriptors);
				}
				var arr = descriptors.ToArray();
				if (arr.Length > 1)
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS, arr, ESelectItemDialogBehavior.SELECT_MULTIPLE | ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
				}
				return ((ICanbeClosed)arr[0].Essence).Close(_creature, liveMapCell);
			}
		}
	}
}