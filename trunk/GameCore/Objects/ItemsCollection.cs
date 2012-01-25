using System;
using System.Collections.Generic;

namespace GameCore.Objects
{
	public class ItemsCollection
	{
		private readonly List<Item> m_items = new List<Item>();

		public List<Item> Items
		{
			get { return m_items; }
		}

		public void Add(Item _item)
		{
			Items.Add(_item);
		}

		public void Remove(Item _item)
		{
			if (!Items.Remove(_item))
			{
				throw new ApplicationException("Такого предмета в инвентаре нет.");
			}
		}
	}
}