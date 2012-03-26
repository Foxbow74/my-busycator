using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public class FakedFurniture : FurnitureThing, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedFurniture(ETiles _tile, Material _material) : base(_material) { m_tile = _tile; }

		public override ETiles Tile { get { return m_tile; } }

		public override string Name { get { throw new NotImplementedException(); } }

		public override EThingCategory Category { get { throw new NotImplementedException(); } }
		public override EMaterial AllowedMaterials { get { throw new NotImplementedException(); } }

		#region IFaked Members

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return ThingHelper.ResolveThing(type, Material, _creature);
		}

		#endregion

		public override bool Is<T>() { return m_types.All(_type => typeof (T).IsAssignableFrom(_type)); }

		public void Add(Type _type) { m_types.Add(_type); }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		protected override int CalcHashCode() { return (int) m_tile; }
	}

	public class FakedCreature : Creature, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedCreature(ETiles _tile)
			: base(null, int.MinValue) { m_tile = _tile; }

		public override ETiles Tile { get { return m_tile; } }

		public override string Name { get { throw new NotImplementedException(); } }

		#region IFaked Members

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];

			var thing = (Thing) Activator.CreateInstance(type, new object[] {_creature.Layer,});
			thing.Resolve(_creature);
			return thing;
		}

		#endregion

		public void Add(Type _type) { m_types.Add(_type); }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }
	}
}