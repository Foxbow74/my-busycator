#region

using System;
using System.Collections.Generic;
using GameCore.Misc;
using GameCore.Objects;

#endregion

namespace GameCore.Creatures
{
	public abstract class Intelligent : Creature
	{
		protected Intelligent(World _world, Point _coords, int _speed) : base(_world, _coords, _speed)
		{
			Inventory = new Inventory();
		}

		public Inventory Inventory { get; private set; }

		public void ObjectTaken(Item _item)
		{
			Inventory.Add(_item);
		}
	}

	public class Inventory
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