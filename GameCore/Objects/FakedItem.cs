using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public class FakedItem : Item, IFaked
	{
		private readonly ETileset m_tileset;
		private readonly List<Type> m_types = new List<Type>();

		public FakedItem(ETileset _tileset, Material _material) : base(_material) { m_tileset = _tileset; }

		public override ETileset Tileset { get { return m_tileset; } }

		public override string Name { get { throw new NotImplementedException(); } }

		public override EThingCategory Category { get { throw new NotImplementedException(); } }

		#region IFaked Members

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return ThingHelper.ResolveThing(type, Material, _creature);
		}

		#endregion

		public void Add(Type _type) { m_types.Add(_type); }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		protected override int CalcHashCode() { return (int) m_tileset; }

		public override bool Is<T>()
		{
			return m_types.Any(_type => typeof (T).IsAssignableFrom(_type));
		}
	}
}