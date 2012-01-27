using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts
{
	public class OpenAct : Act
	{
		public OpenAct() : base(40)
		{
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
					if (cc.Thing.IsClosed(cc, _creature))
					{
						list.Add(cc.WorldCoords);
					}
					else if (cc.GetAllAvailableItems(_creature).Where(_descriptor => _descriptor.Thing.IsClosed(cc, _creature)).Any())
					{
						list.Add(cc.WorldCoords);
					}
				}
				if (_creature.GetBackPackItems().Where(_descriptor => _descriptor.Thing.IsClosed(null, _creature)).Any())
				{
					list.Add(_creature.Coords);
				}

				var coords = list.Distinct();

				if(GetParameter<Point>().Any())
				{
					coords = coords.Intersect(GetParameter<Point>());
				}

				if (!coords.Any())
				{
					//если нечего открывать
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));
					return EActResults.NOTHING_HAPPENS;
				}
				if (coords.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskDirectionMessage(this));
				}
				mapCell = Map.GetMapCell(coords.First());
			}

			//выясняем, что нужно открыть
			{
				var list = new List<ThingDescriptor>();
				if (mapCell.Thing.IsDoor(mapCell, _creature) && mapCell.Thing.IsClosed(mapCell, _creature))
				{
					list.Add(new ThingDescriptor(mapCell.Thing, mapCell.WorldCoords, null));
				}
				list.AddRange(mapCell.GetAllAvailableItems(_creature).Where(_descriptor => _descriptor.Thing.IsClosed(mapCell, _creature)));
				if (mapCell.WorldCoords == _creature.Coords)
				{
					list.AddRange(_creature.GetBackPackItems().Where(_descriptor => _descriptor.Thing.IsClosed(mapCell, _creature)));
				}
				var descriptors = list.Distinct();
				if(GetParameter<ThingDescriptor>().Any())
				{
					descriptors = descriptors.Intersect(GetParameter<ThingDescriptor>());
				}
				if (descriptors.Count() > 1)
				{
					MessageManager.SendMessage(this, new AskSelectThingsMessage(descriptors, this));
				}
				return ((ICanbeOpened) descriptors.First().Thing).Open(_creature, mapCell, _silence);
			}
		}
	}
}