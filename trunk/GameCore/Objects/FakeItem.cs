using System;
using Graphics;

namespace GameCore.Objects
{
	public class FakeItem : Object
	{
		public EItems ItemType { get; private set; }

		public FakeItem(EItems _item)
		{
			ItemType = _item;
		}

		public override Tile Tile
		{
			get { return ItemType.Tile(); }
		}

		public override string Name
		{
			get { throw new System.NotImplementedException(); }
		}

		public Item Resolve()
		{
			switch (ItemType)
			{
				case EItems.NONE:
					break;
				case EItems.WEAPON:
					return new Weapon();
					break;
				case EItems.CHEST:
					break;
				case EItems.DOOR:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			throw new NotImplementedException();
		}

		public static readonly FakeItem Weapon = new FakeItem(EItems.WEAPON);
		public static readonly FakeItem Chest = new FakeItem(EItems.CHEST);
		public static readonly FakeItem Door = new FakeItem(EItems.DOOR);
	}
}