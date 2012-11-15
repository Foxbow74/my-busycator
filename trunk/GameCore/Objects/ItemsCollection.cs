using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public class ItemsCollection
	{
		private readonly List<Item> m_items = new List<Item>();

		public IEnumerable<Item> Items
		{
			get
			{
				foreach (var item in m_items)
				{
					if (item is Stacked)
					{
						var stack = (Stacked) item;
						for (var i = 0; i < stack.Count; ++i)
						{
							yield return stack.Item;
						}
					}
					else
					{
						yield return item;
					}
				}
			}
		}

		public int Count { get { return m_items.Count; } }

		public bool Any { get { return m_items.Count > 0; } }

		public void Add(Item _item)
		{
			var have = m_items.FirstOrDefault(_item1 => _item1.GetHashCode() == _item.GetHashCode());
			if (have == null)
			{
				m_items.Add(_item);
			}
			else if (have is StackOfItems)
			{
				((StackOfItems) have).Add((StackOfItems) _item);
			}
			else if (have is Stacked)
			{
				((Stacked) have).Count++;
			}
			else
			{
				m_items.Remove(have);
				m_items.Add(new Stacked(_item, 2));
			}
		}

		public void Remove(Item _item)
		{
			var have = m_items.FirstOrDefault(_item1 => _item1.GetHashCode() == _item.GetHashCode());
			if (have == null)
			{
				throw new ApplicationException("Такого предмета нет.");
			}
			if (have is Stacked)
			{
				((Stacked) have).Count--;
				if (((Stacked) have).Count == 0)
				{
					m_items.Remove(have);
				}
			}
			else
			{
				m_items.Remove(have);
			}
		}

		#region Nested type: Stacked

		private class Stacked : Item, ISpecial
		{
			public Stacked(Item _item, int _count) : base(_item.Material)
			{
				Item = _item;
				Count = _count;
			}

			public int Count { get; set; }

			public override EThingCategory Category { get { throw new NotImplementedException(); } }

			public override ETiles Tileset { get { throw new NotImplementedException(); } }

			public override string Name { get { throw new NotImplementedException(); } }

			public Item Item { get; private set; }

			public override int GetHashCode() { return Item.GetHashCode(); }

			public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

			public override string ToString() { return "stack of " + Item.GetName(World.TheWorld.Avatar) + " (" + Count + ")"; }
		}

		#endregion
	}
}