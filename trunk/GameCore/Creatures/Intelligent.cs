using System;
using System.Collections.Generic;
using GameCore.Objects;
using Graphics;

namespace GameCore.Creatures
{
	public abstract class Intelligent:Creature
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

		public void Add(Item _item)
		{
			m_items.Add(_item);
		}

		public void Remove(Item _item)
		{
			if(!m_items.Remove(_item))
			{
				throw new ApplicationException("Такого предмета в инвентаре нет.");
			}
		}
	}
}
