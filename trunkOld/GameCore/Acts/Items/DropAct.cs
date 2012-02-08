using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Objects;

namespace GameCore.Acts.Items
{
	class DropAct:Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 10; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get
			{
				yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.D, EKeyModifiers.NONE);
			}
		}

		public override string Name
		{
			get
			{
				return "выбросить предмет";
			}
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get
			{
				return EActionCategory.ITEMS;
			}
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var intelligent = (Intelligent) _creature;
			var descriptors = GetParameter<ThingDescriptor>().ToArray();
			if (descriptors.Length==0)
			{
				MessageManager.SendMessage(this, new AskSelectThingsFromBackPackMessage(this, ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER | ESelectItemDialogBehavior.SELECT_ONE, null));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}
			
			if(descriptors.Length>1)
			{
				foreach (var thingDescriptor in descriptors)
				{
					_creature.AddActToPool(new DropAct(), thingDescriptor);
				}
				return EActResults.NOTHING_HAPPENS;
			}

			var descriptor = descriptors[0];

			if(descriptor == ThingDescriptor.Empty)
			{
				return EActResults.NOTHING_HAPPENS;
			}

			var total = intelligent.GetBackPackItems().Where(_thingDescriptor => _thingDescriptor.Thing.Equals(descriptor)).Count();
			var toTake = 1;
			if(total==0)
			{
				throw new ApplicationException("в рюкзаке нет такого предмета");
			}
			if (total > 1)
			{
				if(GetParameter<int>().Any())
				{
					toTake = GetParameter<int>().Single();
				}
				else
				{
					MessageManager.SendMessage(this, new AskHowMuchMessage(this, descriptor, total));
					return EActResults.NEED_ADDITIONAL_PARAMETERS;
				}
			}
			var item = (Item)descriptor.Thing;
			for (var i = 0; i < toTake; ++i)
			{
				intelligent.ObjectDropedFromBackpack(item);
				Map.GetMapCell(_creature.Coords).AddObjectToBlock(item);
			}
			return EActResults.DONE;
		}
	}
}
