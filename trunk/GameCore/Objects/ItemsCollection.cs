using System;
using System.Linq;
using System.Collections.Generic;

namespace GameCore.Objects
{
	public class ItemsCollection
	{
		private readonly List<Item> m_items = new List<Item>();

		public Item[] Items
		{
			get { return m_items.ToArray(); }
		}

		public int Count
		{
			get { return m_items.Count; }
		}

		public bool Any
		{
			get { return m_items.Count > 0; }
		}

		public void Add(Item _item)
		{
			m_items.Add(_item);
		}

		public void Remove(Item _item)
		{
			var first = m_items.FirstOrDefault(_item1 => _item1.GetHashCode() == _item.GetHashCode());
			if (!m_items.Remove(first))
			{
				throw new ApplicationException("Такого предмета нет.");
			}
		}
	}
}