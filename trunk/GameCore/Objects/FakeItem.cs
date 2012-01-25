#region

using System;
using GameCore.Creatures;
using GameCore.Map;

#endregion

namespace GameCore.Objects
{
	public class FakeItem : Thing, ICanbeOpened
	{
		public static readonly FakeItem Sword = new FakeItem(EItems.SWORD);
		public static readonly FakeItem Axe = new FakeItem(EItems.AXE);
		public static readonly FakeItem Chest = new FakeItem(EItems.CHEST);
		public static readonly FakeItem Door = new FakeItem(EItems.DOOR);

		public FakeItem(EItems _item)
		{
			ItemType = _item;
		}

		public EItems ItemType { get; private set; }

		public override ETiles Tile
		{
			get { return ItemType.Tile(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		#region ICanbeOpened Members

		public bool IsClosed
		{
			get
			{
				switch (ItemType)
				{
					case EItems.CHEST:
					case EItems.DOOR:
						return true;
						break;
					default:
						return false;
				}
			}
		}

		public void Open(Creature _creature, MapCell _mapCell)
		{
			throw new ApplicationException();
		}

		#endregion

		public Thing Resolve()
		{
			switch (ItemType)
			{
				case EItems.NONE:
					break;
				case EItems.SWORD:
					return new Sword();
				case EItems.AXE:
					return new Axe();
				case EItems.CHEST:
					return new Chest();
				case EItems.DOOR:
					return new Door();
				default:
					throw new ArgumentOutOfRangeException();
			}
			throw new NotImplementedException();
		}
	}
}