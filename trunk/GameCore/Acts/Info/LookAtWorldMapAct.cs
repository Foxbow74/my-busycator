using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Messages;

namespace GameCore.Acts.Info
{
	public class LookAtWorldMapAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 0; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.M, EKeyModifiers.CTRL); } }

		public override EALConst Name { get { return EALConst.AN_WORLD_MAP; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.INFORMATION; } }

		public override EActResults Do(Creature _creature)
		{
			MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.WORLD_MAP));
			return EActResults.DONE;
		}
	}
}