namespace GameCore.Objects
{
	public abstract class Container : Item
	{
		private ItemsCollection m_items;

		public ItemsCollection Items
		{
			get { return m_items ?? (m_items = new ItemsCollection()); }
		}

	}

	public enum LockType
	{
		OPEN,
		SIMPLE,
	}
}