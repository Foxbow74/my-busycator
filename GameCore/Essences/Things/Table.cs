﻿using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	internal class Table : Thing
	{
		public Table(Material _material) : base(EALNouns.Table, _material) { }

        public override int TileIndex { get { return 12; } }
	}
}