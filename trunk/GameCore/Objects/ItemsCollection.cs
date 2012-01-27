using System;
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
			if (!m_items.Remove(_item))
			{
				throw new ApplicationException("Такого предмета нет.");
			}
		}
	}
}