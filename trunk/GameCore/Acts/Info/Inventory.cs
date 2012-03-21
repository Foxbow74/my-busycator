using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Messages;

namespace GameCore.Acts.Info
{
	public class InventoryAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 0; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.I, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "показать инвентарь"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.INFORMATION; }
		}

		public override EActResults Do(Creature _creature)
		{
			MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.INVENTORY));
			return EActResults.DONE;
		}
	}
}