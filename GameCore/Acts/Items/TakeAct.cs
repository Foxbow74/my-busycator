using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts.Items
{
	public class TakeAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 20; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.OemComma, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "подобрать предмет"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.ITEMS; }
		}

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			var notTaken = _creature.GetNotTakenAvailableItems(GetParameter<Point>());

			if (!notTaken.Any())
			{
				// Если ничего не доступно
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "тут ничего нет."));
				}
				return EActResults.QUICK_FAIL;
			}

			var toTake = GetParameter<ThingDescriptor>().ToList();

			if (toTake.Any(_thingDescriptor => _thingDescriptor == ThingDescriptor.Empty))
			{
				return EActResults.QUICK_FAIL;
			}

			if (toTake.Count == 0)
			{
				// Если в параметрах нет уточнений
				if (notTaken.Count() == 1)
				{
					var needToShow = GetParameter<bool>();
					if (needToShow.Any() && needToShow.Single())
					{
						MessageManager.SendMessage(this,
						                           new AskSelectThingsMessage(notTaken, this,
						                                                      ESelectItemDialogBehavior.SELECT_MULTIPLE |
						                                                      ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
						return EActResults.NEED_ADDITIONAL_PARAMETERS;
					}

					toTake.AddRange(notTaken);
				}
				else
				{
					//получаем все возможные координаты ячеек, где лежат не взятые предметы, ограничив параметром направления, если таковой представлен
					var coords = notTaken.Select(_descriptor => _descriptor.LiveCoords).Distinct();
					if (coords.Count() > 1)
					{
						MessageManager.SendMessage(this, new AskDirectionMessage(this, _creature.LiveCoords));
					}
					else
					{
						MessageManager.SendMessage(this,
						                           new AskSelectThingsMessage(notTaken, this,
						                                                      ESelectItemDialogBehavior.SELECT_MULTIPLE |
						                                                      ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER));
					}
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
			}

			if (toTake.Count > 1)
			{
				foreach (var descr in toTake)
				{
					_creature.AddActToPool(new TakeAct(), GetParameter<Point>(), descr);
				}
				return EActResults.ACT_REPLACED;
			}

			var descriptor = toTake[0];
			var thing = descriptor.ResolveThing(_creature);
			var thingString = thing.GetName(_creature);
			IEnumerable<Item> get;
			if (descriptor.Container == null)
			{
				get = World.TheWorld.LiveMap.GetCell(descriptor.LiveCoords).Items.Where(_item => _item.Equals(thing));
			}
			else
			{
				get = descriptor.Container.GetItems(_creature).Items.Where(_item => _item.Equals(thing));
			}

			if (get.Count() > 1)
			{
				var cnt = GetParameter<int>();
				if (cnt.Any())
				{
					Count = cnt.Single();
				}
				else
				{
					MessageManager.SendMessage(this, new AskHowMuchMessage(this, descriptor, get.Count()));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
			}

			for (var i = 0; i < Count; ++i)
			{
				if (descriptor.Container == null)
				{
					World.TheWorld.LiveMap.GetCell(descriptor.LiveCoords).RemoveItem((Item) thing);
				}
				else
				{
					descriptor.Container.GetItems(_creature).Remove((Item) thing);
				}
			}

			for (var i = 0; i < Count; ++i)
			{
				intelligent.ObjectTaken((Item) thing);
			}
			if (_creature.IsAvatar && Count > 0)
			{
				var suffix = Count > 1 ? (", " + Count + " штук.") : ".";
				if (intelligent.IsAvatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, thingString + " взят" + suffix));
				}
				else
				{
					MessageManager.SendMessage(this,
					                           new SimpleTextMessage(EMessageType.INFO, _creature + " взял " + thingString + suffix));
				}
			}
			return EActResults.DONE;
		}
	}
}