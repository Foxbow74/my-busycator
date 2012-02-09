using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	public interface IContainer
	{
		ItemsCollection GetItems(Creature _creature);
	}

	internal abstract class Container : Furniture, IContainer
	{
		private ItemsCollection m_items;

		#region IContainer Members

		/// <summary>
		/// 	Если контейнер не инициализирован, то на основании характеристик открывшего существа генерируется наполнение
		/// </summary>
		/// <param name = "_creature"></param>
		/// <returns></returns>
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

		#endregion

		protected virtual IEnumerable<Item> GenerateItems(Creature _creature)
		{
			yield break;
		}
	}

	public enum ELockType
	{
		OPEN,
		SIMPLE,
	}
}