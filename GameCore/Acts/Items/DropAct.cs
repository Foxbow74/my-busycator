using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Essences;
using RusLanguage;

namespace GameCore.Acts.Items
{
	internal class DropAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 10; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.D, EKeyModifiers.NONE); } }

		public override string Name { get { return "выбросить предмет"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.ITEMS; } }

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			var descriptors = GetParameter<EssenceDescriptor>().ToArray();
			if (descriptors.Length == 0)
			{
				MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS_FROM_BACK_PACK, ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER | ESelectItemDialogBehavior.SELECT_ONE));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}

			if (descriptors.Length > 1)
			{
				foreach (var thingDescriptor in descriptors)
				{
					_creature.AddActToPool(new DropAct(), thingDescriptor);
				}
				return EActResults.QUICK_FAIL;
			}

			var descriptor = descriptors[0];

			if (descriptor == EssenceDescriptor.Empty)
			{
				return EActResults.QUICK_FAIL;
			}

			var total = intelligent.GetBackPackItems().Where(_thingDescriptor => _thingDescriptor.GetName(_creature) == descriptor.GetName(_creature)).Count();
			if (total == 0)
			{
				throw new ApplicationException("в рюкзаке нет такого предмета");
			}
			if (total > 1)
			{
				int cnt;
				if (TryGetParameter(out cnt))
				{
					Count = cnt;
				}
				else
				{
					MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.HOW_MUCH, descriptor, total));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
			}
			else
			{
				Count = 1;
			}
			var item = (Item) descriptor.Essence;
			for (var i = 0; i < Count; ++i)
			{
				intelligent.RemoveFromBackpack(item);
				_creature[0, 0].AddItem(item);
			}
			var suffix = Count > 1 ? (", " + Count + " штук.") : ".";
			if (intelligent.IsAvatar)
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, item[EPadej.IMEN] + " выброшен" + suffix));
			}
			else
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " выбросил " + item[EPadej.VIN] + suffix));
			}
			return EActResults.DONE;
		}
	}
}