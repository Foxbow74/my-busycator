using System;
using GameCore.Misc;

namespace GameCore.Objects
{
	public abstract class Container:Item
	{
	}

	public class Chest : Container, ICanbeOpened
	{
		public override ETiles Tile
		{
			get { return ETiles.CHEST; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}

		public bool IsClosed
		{
			get { throw new NotImplementedException(); }
		}
	}
}
