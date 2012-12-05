using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Messages;

namespace GameCore.Acts.Info
{
	public class HelpAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 0; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Oem2, EKeyModifiers.SHIFT); } }

		public override EALConst Name { get { return EALConst.AN_HELP; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.INFORMATION; } }

		public override EActResults Do(Creature _creature)
		{
			MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.HELP));
			return EActResults.DONE;
		}
	}
}