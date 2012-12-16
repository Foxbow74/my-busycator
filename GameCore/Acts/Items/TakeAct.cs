using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Acts.Items
{
	public class TakeAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 20; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.OemComma, EKeyModifiers.NONE); } }

		public override EALConst Name { get { return EALConst.AN_TAKE; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.ITEMS; } }

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			var notTaken = _creature.GetNotTakenAvailableItems(GetParameter<Point>().ToArray());

			if (!notTaken.Any())
			{
				// Если ничего не доступно
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "тут ничего нет."));
				}
				return EActResults.QUICK_FAIL;
			}

			var toTake = GetParameter<EssenceDescriptor>().ToList();

			if (toTake.Any(_thingDescriptor => _thingDescriptor == EssenceDescriptor.Empty))
			{
				return EActResults.QUICK_FAIL;
			}

			if (toTake.Count == 0)
			{
				// Если в параметрах нет уточнений
				var ask = new AskMessageNg(this, EAskMessageType.SELECT_THINGS, ESelectItemDialogBehavior.SELECT_MULTIPLE | ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER);
				foreach (var essenceDescriptor in notTaken)
				{
					ask.AddParameter(essenceDescriptor);
				}
				if (notTaken.Count() == 1)
				{
					var needToShow = GetParameter<bool>().ToArray();
					if (needToShow.Length>0 && needToShow[0])
					{
						MessageManager.SendMessage(this, ask);
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
						MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.ASK_DIRECTION));
					}
					else
					{
						MessageManager.SendMessage(this, ask);
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
			var thing = descriptor.ResolveEssence(_creature);
			Item[] get;
			if (descriptor.Container == null)
			{
				get = World.TheWorld.LiveMap.GetCell(descriptor.LiveCoords).Items.Where(_item => _item.Equals(thing)).ToArray();
			}
			else
			{
				get = descriptor.Container.GetItems(_creature).Items.Where(_item => _item.Equals(thing)).ToArray();
			}

			if (get.Count() > 1)
			{
				var cnt = GetParameter<int>().ToArray();
				if (cnt.Length>0)
				{
					Count = cnt[0];
				}
				else
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.HOW_MUCH, descriptor, get.Count()));
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

			MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURE_TAKES_IT, _creature, thing, Count));

			return EActResults.DONE;
		}
	}
}