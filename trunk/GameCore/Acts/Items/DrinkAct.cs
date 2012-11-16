using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Essences;
using GameCore.Essences.Potions;
using GameCore.Essences.Tools;
using RusLanguage;

namespace GameCore.Acts.Items
{
	internal class DrinkAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 30; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Q, EKeyModifiers.NONE); } }

		public override string Name { get { return "выпить"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.ITEMS; } }

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			var descriptors = GetParameter<EssenceDescriptor>().ToArray();
			if (descriptors.Length == 0)
			{
				MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.SELECT_THINGS_FROM_BACK_PACK, ESelectItemDialogBehavior.SELECT_ONE, new[] {EEssenceCategory.POTION}));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}
			var descriptor = descriptors[0];
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

			if (intelligent.IsAvatar)
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, item.Name + " выпит"));
			}
			else
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " выпил " + item[EPadej.VIN]));
			}
			return EActResults.DONE;
		}
	}

	internal class UseTool : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 30; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.U, EKeyModifiers.NONE); } }

		public override string Name { get { return "задействовать инструмент"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.ITEMS; } }

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			var tool = intelligent[EEquipmentPlaces.TOOL];

			if (tool == null)
			{
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Ни один инструмент не экипирован!"));
				}
				return EActResults.QUICK_FAIL;
			}

			((ITool) tool).UseTool(intelligent);

			return EActResults.DONE;
		}
	}
}