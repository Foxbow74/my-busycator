using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Objects;
using GameCore.Objects.Potions;

namespace GameCore.Acts.Items
{
	internal class DrinkAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 30; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Q, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "выпить"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.ITEMS; }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var intelligent = (Intelligent) _creature;
			var descriptors = GetParameter<ThingDescriptor>().ToArray();
			if (descriptors.Length == 0)
			{
				MessageManager.SendMessage(this,
				                           new AskSelectThingsFromBackPackMessage(this, ESelectItemDialogBehavior.SELECT_ONE,
				                                                                  new[] {EThingCategory.POTION}));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}
			var descriptor = descriptors[0];
			if (descriptor == ThingDescriptor.Empty)
			{
				return EActResults.NOTHING_HAPPENS;
			}
			var total =
				intelligent.GetBackPackItems().Where(_thingDescriptor => _thingDescriptor.Thing.Equals(descriptor)).ToArray();
			if (total.Length == 0)
			{
				throw new ApplicationException("в рюкзаке нет такого предмета");
			}
			var item = (Potion) descriptor.Thing;
			var thingString = item.ToString();

			if (!item.IsAllowToDrink(_creature))
			{
				return EActResults.NOTHING_HAPPENS;
			}

			intelligent.RemoveFromBackpack(item);
			item.Drinked(_creature, _silence);

			if (intelligent.IsAvatar)
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, thingString + " выпит"));
			}
			else
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " выпил " + thingString));
			}
			return EActResults.DONE;
		}
	}
}