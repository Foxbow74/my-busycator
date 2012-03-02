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
	public class CloseAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 20; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.C, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "Закрыть сундук/дверь"; }
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
			LiveMapCell liveMapCell; // = Map.GetMapCell(_creature.Coords);
			{
				//собираем координаты всех закрытых вещей
				var list = new List<Point>();
				foreach (var cell in _creature.LiveCoords.NearestDPoints.Select(_point => _creature[_point]))
				{
					var cc = cell;
					if (cc.Furniture.CanBeClosed(cc, _creature))
					{
						list.Add(cc.LiveCoords);
					}
					else if (cc.GetAllAvailableItemDescriptors<FurnitureThing>(_creature).Any(_descriptor => _descriptor.Thing.CanBeClosed(cc, _creature)))
					{
						list.Add(cc.LiveCoords);
					}
				}
				if (_creature.GetBackPackItems().Any(_descriptor => _descriptor.Thing.CanBeClosed(null, _creature)))
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
					//если нечего закрывать
					if (_creature.IsAvatar) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "закрыть что?"));
					return EActResults.NOTHING_HAPPENS;
				}
				if (coords.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskDirectionMessage(this, _creature.LiveCoords));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
				liveMapCell = World.TheWorld.LiveMap.GetCell(coords.First());
			}

			//выясняем, что нужно закрыть
			{
				var list = new List<ThingDescriptor>();
				if (liveMapCell.Furniture.Is<ICanbeClosed>())
				{
					list.Add(new ThingDescriptor(liveMapCell.Furniture, liveMapCell.LiveCoords, null));
				}
				list.AddRange(liveMapCell.GetAllAvailableItemDescriptors<FurnitureThing>(_creature).Where(
						_descriptor => _descriptor.Thing.CanBeClosed(liveMapCell, _creature)));
				if (liveMapCell.LiveCoords == _creature.LiveCoords)
				{
					list.AddRange(_creature.GetBackPackItems().Where(_descriptor => _descriptor.Thing.CanBeClosed(liveMapCell, _creature)));
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
				return ((ICanbeClosed) descriptors.First().Thing).Close(_creature, liveMapCell);
			}
		}
	}
}