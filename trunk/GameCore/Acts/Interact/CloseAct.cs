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

		public override EActResults Do(Creature _creature, bool _silence)
		{
			MapCell mapCell; // = Map.GetMapCell(_creature.Coords);
			{
				//собираем координаты всех закрытых вещей
				var list = new List<Point>();
				foreach (var cell in _creature.Coords.NearestPoints.Select(Map.GetMapCell))
				{
					var cc = cell;
					if (cc.Thing.CanBeClosed(cc, _creature))
					{
						list.Add(cc.WorldCoords);
					}
					else if (cc.GetAllAvailableItems(_creature).Any(_descriptor => _descriptor.Thing.CanBeClosed(cc, _creature)))
					{
						list.Add(cc.WorldCoords);
					}
				}
				if (_creature.GetBackPackItems().Any(_descriptor => _descriptor.Thing.CanBeClosed(null, _creature)))
				{
					list.Add(_creature.Coords);
				}

				var coords = list.Distinct().ToList();

				if (GetParameter<Point>().Any())
				{
					coords = coords.Intersect(GetParameter<Point>()).ToList();
				}

				if (!coords.Any())
				{
					//если нечего закрывать
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "закрыть что?"));
					return EActResults.NOTHING_HAPPENS;
				}
				if (coords.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskDirectionMessage(this, _creature.Coords));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
				mapCell = Map.GetMapCell(coords.First());
			}

			//выясняем, что нужно закрыть
			{
				var list = new List<ThingDescriptor>();
				if ((mapCell.Thing.IsDoor(mapCell, _creature) || mapCell.Thing.IsChest(mapCell, _creature)) &&
				    mapCell.Thing.CanBeClosed(mapCell, _creature))
				{
					list.Add(new ThingDescriptor(mapCell.Thing, mapCell.WorldCoords, null));
				}
				list.AddRange(
					mapCell.GetAllAvailableItems(_creature).Where(_descriptor => _descriptor.Thing.CanBeClosed(mapCell, _creature)));
				if (mapCell.WorldCoords == _creature.Coords)
				{
					list.AddRange(_creature.GetBackPackItems().Where(_descriptor => _descriptor.Thing.CanBeClosed(mapCell, _creature)));
				}
				var descriptors = list.Distinct();
				if (GetParameter<ThingDescriptor>().Any())
				{
					descriptors = GetParameter<ThingDescriptor>().Intersect(descriptors);
				}
				if (descriptors.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskSelectThingsMessage(descriptors, this, ESelectItemDialogBehavior.SELECT_MULTIPLE | ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
				}
				return ((ICanbeClosed) descriptors.First().Thing).Close(_creature, mapCell, _silence);
			}
		}
	}
}