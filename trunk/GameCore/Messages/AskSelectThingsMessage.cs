using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Objects;

namespace GameCore.Messages
{
	public class AskSelectThingsMessage : AskMessage
	{
		public AskSelectThingsMessage(IEnumerable<ThingDescriptor> _items, Act _act, ESelectItemDialogBehavior _behavior)
			: base(_act)
		{
			ItemDescriptors = _items;
			Behavior = _behavior;
		}

		public IEnumerable<ThingDescriptor> ItemDescriptors { get; private set; }

		public ESelectItemDialogBehavior Behavior { get; set; }
	}

	public class AskSelectThingsFromBackPackMessage : AskMessage
	{
		public AskSelectThingsFromBackPackMessage(Act _act, ESelectItemDialogBehavior _behavior, IEnumerable<EThingCategory> _allowedCategory)
			: base(_act)
		{
			Behavior = _behavior;
			AllowedCategory = _allowedCategory;
		}

		public ESelectItemDialogBehavior Behavior { get; set; }
		public IEnumerable<EThingCategory> AllowedCategory { get; set; }
	}
}