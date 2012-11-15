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
			{
				var delta = GetParameter<Point>().FirstOrDefault();
				if (delta == null)
				{
					//собираем координаты всех закрытых вещей
					var list = new List<Point>();
					foreach (var dPoint in Point.NearestDPoints)
					{
						var cell = _creature[dPoint];
						var furniture = cell.Furniture;
						if (furniture.CanBeClosed(cell, _creature))
						{
							if (furniture.Is<OpenDoor>() && dPoint.Lenght == 0)
							{
								continue;
							}
							list.Add(dPoint);
						}
						else if (cell.GetAllAvailableItemDescriptors<Furniture>(_creature).Any(_descriptor => _descriptor.Thing.CanBeClosed(cell, _creature)))
						{
							list.Add(dPoint);
						}
					}
					if (_creature.GetBackPackItems().Any(_descriptor => _descriptor.Thing.CanBeClosed(null, _creature)))
					{
						list.Add(Point.Zero);
					}
					var variants = list.Distinct().ToArray();
					if (variants.Length == 0)
					{
						if (_creature.IsAvatar) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "закрыть что?"));
						return EActResults.QUICK_FAIL;
					}
					if (variants.Length > 1)
					{
						MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.ASK_DIRECTION));
						return EActResults.NEED_ADDITIONAL_PARAMETERS;
					}
					delta = variants[0];
				}
				liveMapCell = _creature[delta];
			}

			//выясняем, что нужно закрыть
			{
				var list = new List<ThingDescriptor>();
				if (liveMapCell.Furniture.Is<ICanbeClosed>())
				{
					list.Add(new ThingDescriptor(liveMapCell.Furniture, liveMapCell.LiveCoords, null));
				}
				list.AddRange(liveMapCell.GetAllAvailableItemDescriptors<Furniture>(_creature).Where(
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
				var arr = descriptors.ToArray();
				if (arr.Length > 1)
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS, arr, ESelectItemDialogBehavior.SELECT_MULTIPLE | ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
				}
				return ((ICanbeClosed)arr[0].Thing).Close(_creature, liveMapCell);
			}
		}
	}
}