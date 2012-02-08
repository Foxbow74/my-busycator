using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public abstract class Container : Thing
	{
		private ItemsCollection m_items;

		public ItemsCollection GetItems(Creature _creature)
		{
			if (m_items == null)
			{
				m_items = new ItemsCollection();
				foreach (var item in GenerateItems(_creature))
				{
					if (item is IFaked)
					{
						m_items.Add((Item) ((IFaked) item).ResolveFake(_creature));
					}
					else
					{
						m_items.Add(item);
					}
				}
			}
			return m_items;
		}


		protected virtual IEnumerable<Item> GenerateItems(Creature _creature)
		{
			yield break;
		}
	}

	public enum LockType
	{
		OPEN,
		SIMPLE,
	}
}