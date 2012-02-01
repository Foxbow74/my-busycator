using System;
using System.Collections.Generic;
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
			var cnt = World.Rnd.Next(_creature.GetLuckRandom);
			for (var i = 0; i < cnt; i++)
			{
				yield return (Item)ThingHelper.GetFaketItem(_creature.MapBlock);
			}
		}
	}
}