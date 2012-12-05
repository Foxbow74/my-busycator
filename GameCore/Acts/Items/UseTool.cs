using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences.Tools;
using GameCore.Messages;

namespace GameCore.Acts.Items
{
	internal class UseTool : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 30; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.U, EKeyModifiers.NONE); } }

		public override EALConst Name { get { return EALConst.AN_USE; } }

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