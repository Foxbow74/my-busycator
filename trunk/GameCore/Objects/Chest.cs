using System;
using GameCore.Creatures;
using GameCore.Map;

namespace GameCore.Objects
{
	public class Chest : Container, ICanbeOpened
	{
		public Chest()
		{
			LockType = LockType.SIMPLE;
		}

		public LockType LockType { get; private set; }

		public override ETiles Tile
		{
			get { return ETiles.CHEST; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}

		#region ICanbeOpened Members

		public bool IsClosed
		{
			get
			{
				switch (LockType)
				{
					case LockType.OPEN:
						return false;
						break;
					case LockType.SIMPLE:
						return true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void Open(Creature _creature, MapCell _mapCell)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}