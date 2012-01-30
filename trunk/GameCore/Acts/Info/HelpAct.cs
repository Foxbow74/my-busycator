using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Acts.Info
{
	class HelpAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.Oem2, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "�������� ����� ������"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.INFORMATION; }
		}
	}
}