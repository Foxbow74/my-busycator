using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Objects.Furniture
{
	public class Heap : Container, ISpecial
	{
		private readonly MapBlock m_block;
		private readonly Point m_localPoint;

		public Heap(MapBlock _block, Point _localPoint)
		{
			m_block = _block;
			m_localPoint = _localPoint;
		}

		public override float Opaque
		{
			get
			{
				return m_block.Objects.Count(_tuple => _tuple.Item2 == m_localPoint)*0.1f;
			}
		}

		public override ETiles Tile
		{
			get { return ETiles.HEAP_OF_ITEMS; }
		}

		public override string Name
		{
			get { return "куча вещей"; }
		}

		public override EThingCategory Category
		{
			get { throw new NotImplementedException(); }
		}

		public override void Resolve(Creature _creature)
		{
		}

		protected override IEnumerable<Item> GenerateItems(Creature _creature)
		{
			var enumerable = m_block.Objects.Where(_tuple => _tuple.Item2 == m_localPoint);
			foreach (var tuple in enumerable)
			{
				if (tuple.Item1 is FakedItem)
				{
					var fake = ((FakedItem) tuple.Item1);
					yield return (Item)fake.ResolveFake(_creature);
				}
				else
				{
					yield return (Item)tuple.Item1;
				}
			}
		}
	}
}