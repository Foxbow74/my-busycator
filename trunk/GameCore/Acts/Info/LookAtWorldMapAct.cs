﻿using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Acts.Info
{
	internal class LookAtWorldMapAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.M, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "посмотреть карту мира"; }
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
			throw new NotImplementedException();
		}
	}
}