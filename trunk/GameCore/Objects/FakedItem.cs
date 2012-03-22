using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public class FakedItem : Item, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedItem(ETiles _tile, Material _material):base(_material)
		{
			m_tile = _tile;
		}

		public override ETiles Tile
		{
			get { return m_tile; }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override EThingCategory Category
		{
			get { throw new NotImplementedException(); }
		}

		#region IFaked Members

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return ThingHelper.ResolveThing(type, Material, _creature);
		}

		#endregion

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}

		protected override int CalcHashCode()
		{
			return (int)m_tile;
		}
	}
}