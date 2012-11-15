using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furnitures;

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
			{
				//собираем координаты всех закрытых вещей
				var list = new List<Point>();
				foreach (var point in Point.NearestDPoints)
				{
					var cc = _creature[point];
					if (cc.Furniture.IsClosed(cc, _creature))
					{
						list.Add(point);
					}
					else if (cc.GetAllAvailableItemDescriptors<Furniture>(_creature).Any(_descriptor => _descriptor.Thing.IsClosed(cc, _creature)))
					{
						list.Add(point);
					}
				}
				if (_creature.GetBackPackItems().Any(_descriptor => _descriptor.Thing.IsClosed(null, _creature)))
				{
					list.Add(Point.Zero);
				}

				var coords = list.Distinct().ToList();

				if (GetParameter<Point>().Any())
				{
					coords = coords.Intersect(GetParameter<Point>()).ToList();
				}

				if (!coords.Any())
				{
					//если нечего открывать
					if (_creature.IsAvatar) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));
					return EActResults.QUICK_FAIL;
				}
				if (coords.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.ASK_DIRECTION));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
				liveMapCell = _creature[coords.First()];
			}

			//выясняем, что нужно открыть
			{
				var list = new List<ThingDescriptor>();
				if ((liveMapCell.Furniture.Is<ClosedDoor>() || liveMapCell.Furniture.Is<Chest>()) && liveMapCell.Furniture.IsClosed(liveMapCell, _creature))
				{
					list.Add(new ThingDescriptor(liveMapCell.Furniture, liveMapCell.LiveCoords, null));
				}
				list.AddRange(liveMapCell.GetAllAvailableItemDescriptors<Furniture>(_creature).Where(_descriptor => _descriptor.Thing.IsClosed(liveMapCell, _creature)));
				if (liveMapCell.LiveCoords == _creature.LiveCoords)
				{
					list.AddRange(_creature.GetBackPackItems().Where(_descriptor => _descriptor.Thing.IsClosed(liveMapCell, _creature)));
				}
				var descriptors = list.Distinct();
				if (GetParameter<ThingDescriptor>().Any())
				{
					descriptors = GetParameter<ThingDescriptor>().Intersect(descriptors);
				}
				if (descriptors.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS, descriptors, ESelectItemDialogBehavior.SELECT_MULTIPLE | ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
				}
				return ((ICanbeOpened) descriptors.First().Thing).Open(_creature, liveMapCell);
			}
		}
	}
}