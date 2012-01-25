using System;
using GameCore.Misc;

namespace GameCore.Objects
{
	public class FakeItem : Object
	{
		public EItems ItemType { get; private set; }

		public FakeItem(EItems _item)
		{
			ItemType = _item;
		}

		public override ETiles Tile
		{
			get { return ItemType.Tile(); }
		}

		public override string Name
		{
			get { throw new System.NotImplementedException(); }
		}

		public Object Resolve()
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

		public static readonly FakeItem Sword = new FakeItem(EItems.SWORD);
		public static readonly FakeItem Axe = new FakeItem(EItems.AXE);
		public static readonly FakeItem Chest = new FakeItem(EItems.CHEST);
		public static readonly FakeItem Door = new FakeItem(EItems.DOOR);
	}
}