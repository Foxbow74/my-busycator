namespace GameCore.Objects
{
	public class Object
	{
		public Object()
		{
		}
	}

	public class Item : Object
	{
		public Item()
		{
		}
	}

	public class FakeItem : Object
	{
		public EItems Item { get; private set; }

		public FakeItem(EItems _item)
		{
			Item = _item;
		}
	}

	public class Weapon : Item
	{
		public Weapon()
		{
		}
	}

	public static class FakeItems
	{
		public static readonly FakeItem Weapon = new FakeItem(EItems.WEAPON);
		public static readonly FakeItem Chest = new FakeItem(EItems.CHEST);
		public static readonly FakeItem Door = new FakeItem(EItems.DOOR);
	}
}
