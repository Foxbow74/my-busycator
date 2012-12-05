using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Messages;

namespace GameCore.Acts.Items
{
	internal class DrinkAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 30; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Q, EKeyModifiers.NONE); } }

		public override EALConst Name { get { return EALConst.AN_DRINK; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.ITEMS; } }

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			EssenceDescriptor descriptor;
			if (!TryGetParameter(out descriptor))
			{
				MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS_FROM_BACK_PACK, ESelectItemDialogBehavior.SELECT_ONE, new[] {EItemCategory.POTION}));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}
			if (descriptor == EssenceDescriptor.Empty)
			{
				return EActResults.QUICK_FAIL;
			}
			var total = intelligent.GetBackPackItems().Where(_thingDescriptor => _thingDescriptor.Essence.Equals(descriptor)).ToArray();

			if (total.Length == 0)
			{
				throw new ApplicationException("в рюкзаке нет такого предмета");
			}
			var item = (Potion) descriptor.Essence;

			if (!item.IsAllowToDrink(_creature))
			{
				return EActResults.QUICK_FAIL;
			}

			intelligent.RemoveFromBackpack(item);
			item.Drinked(_creature);

			MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURE_DRINKS_IT, _creature, item));
			return EActResults.DONE;
		}
	}
}