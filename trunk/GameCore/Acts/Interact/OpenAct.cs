using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts.Interact
{
	public class OpenAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 40; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.O, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "Открыть сундук/дверь"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.WORLD_INTERACTIONS; }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			LiveMapCell liveMapCell; // = Map.GetMapCell(_creature.Coords);
			{
				//собираем координаты всех закрытых вещей
				var list = new List<Point>();
				foreach (var cell in _creature.LiveCoords.NearestDPoints.Select(_point => _creature[_point]))
				{
					var cc = cell;
					if (cc.Furniture.IsClosed(cc, _creature))
					{
						list.Add(cc.LiveCoords);
					}
					else if (cc.GetAllAvailableItemDescriptors(_creature).Any(_descriptor => _descriptor.Thing.IsClosed(cc, _creature)))
					{
						list.Add(cc.LiveCoords);
					}
				}
				if (_creature.GetBackPackItems().Any(_descriptor => _descriptor.Thing.IsClosed(null, _creature)))
				{
					list.Add(_creature.LiveCoords);
				}

				var coords = list.Distinct().ToList();

				if (GetParameter<Point>().Any())
				{
					coords = coords.Intersect(GetParameter<Point>()).ToList();
				}

				if (!coords.Any())
				{
					//если нечего открывать
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));
					return EActResults.NOTHING_HAPPENS;
				}
				if (coords.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskDirectionMessage(this, _creature.LiveCoords));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
				liveMapCell = World.TheWorld.LiveMap.GetCell(coords.First());
			}

			//выясняем, что нужно открыть
			{
				var list = new List<ThingDescriptor>();
				if ((liveMapCell.Furniture.IsDoor(liveMapCell, _creature) || liveMapCell.Furniture.IsChest(liveMapCell, _creature)) &&
				    liveMapCell.Furniture.IsClosed(liveMapCell, _creature))
				{
					list.Add(new ThingDescriptor(liveMapCell.Furniture, liveMapCell.LiveCoords, null));
				}
				list.AddRange(
					liveMapCell.GetAllAvailableItemDescriptors(_creature).Where(
						_descriptor => _descriptor.Thing.IsClosed(liveMapCell, _creature)));
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
					MessageManager.SendMessage(this,
					                           new AskSelectThingsMessage(descriptors, this,
					                                                      ESelectItemDialogBehavior.SELECT_MULTIPLE |
					                                                      ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
				}
				return ((ICanbeOpened) descriptors.First().Thing).Open(_creature, liveMapCell, _silence);
			}
		}
	}
}