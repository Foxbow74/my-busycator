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
				foreach (var faked in GenerateItems(_creature))
				{
					m_items.Add((Item)faked.ResolveFake(_creature));
				}
			}
			return m_items;
		}

		protected virtual IEnumerable<IFaked> GenerateItems(Creature _creature)
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