using System;
using GameCore.Misc;

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		bool IsClosed { get; }
	}

	class Door : Object, ICanbeOpened
	{
		public override ETiles Tile
		{
			get { return ETiles.DOOR; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}

		public bool IsClosed
		{
			get { throw new NotImplementedException(); }
		}
	}
}
