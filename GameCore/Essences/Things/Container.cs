using System.Collections.Generic;
using System.Diagnostics;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences.Faked;

namespace GameCore.Essences.Things
{
	public interface IContainer
	{
		ItemsCollection GetItems(Creature _creature);
		Essence ResolveFakeItem(Creature _creature, FakedItem _fakedItem);
	}

	internal abstract class Container : Thing, IContainer
	{
		private ItemsCollection m_items;

		protected Container(EALNouns _name, Material _material) : base(_name, _material) { }

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
						m_items.Add((Item) ((IFaked) item).Essence.Clone(_creature));
					}
					else
					{
						m_items.Add(item);
					}
				}
			}
			return m_items;
		}

		public Essence ResolveFakeItem(Creature _creature, FakedItem _fakedItem)
		{
			Debug.WriteLine("RESOLVE " + _fakedItem);
			GetItems(_creature).Remove(_fakedItem);
			var item = (Item)_fakedItem.Essence.Clone(_creature);
			GetItems(_creature).Add(item);
			return item;
		}

		#endregion

		protected virtual IEnumerable<Item> GenerateItems(Creature _creature) { yield break; }
	}

	public enum ELockType
	{
		OPEN,
		SIMPLE,
	}
}