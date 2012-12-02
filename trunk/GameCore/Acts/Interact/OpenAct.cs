using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Acts.Interact
{
	public class OpenAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 200; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.O, EKeyModifiers.NONE); } }

		public override string Name { get { return "Открыть сундук/дверь"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.WORLD_INTERACTIONS; } }

		public override EActResults Do(Creature _creature)
		{
			LiveMapCell liveMapCell;

			var find = Find(_creature, (_essence, _cell) => _essence.IsLockedFor(_cell, _creature), out liveMapCell);
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
			
			//выясняем, что нужно открыть
			{
				var list = new List<EssenceDescriptor>();
				if ((liveMapCell.Thing.Is<ClosedDoor>() || liveMapCell.Thing.Is<Chest>()) && liveMapCell.Thing.IsLockedFor(liveMapCell, _creature))
				{
					list.Add(new EssenceDescriptor(liveMapCell.Thing, liveMapCell.LiveCoords, null));
				}
				list.AddRange(liveMapCell.GetAllAvailableItemDescriptors<Thing>(_creature).Where(_descriptor => _descriptor.Essence.IsLockedFor(liveMapCell, _creature)));
				if (liveMapCell.LiveCoords == _creature.GeoInfo.LiveCoords)
				{
					list.AddRange(_creature.GetBackPackItems().Where(_descriptor => _descriptor.Essence.IsLockedFor(liveMapCell, _creature)));
				}
				var descriptors = list.Distinct();
				if (GetParameter<EssenceDescriptor>().Any())
				{
					descriptors = GetParameter<EssenceDescriptor>().Intersect(descriptors);
				}
				if (descriptors.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS, descriptors, ESelectItemDialogBehavior.SELECT_MULTIPLE | ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
				}
				return ((ICanbeOpened) descriptors.First().Essence).Open(_creature, liveMapCell);
			}
		}
	}
}