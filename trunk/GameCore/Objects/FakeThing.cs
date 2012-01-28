using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Objects
{
	public interface IFaked
	{
		Thing ResolveFake(Creature _creature);
	}

	public class FakedThing : Thing, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedThing(ETiles _tile)
		{
			m_tile = _tile;
		}

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override ETiles Tile
		{
			get { return m_tile; }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return ThingHelper.ResolveThing(type, _creature);
		}
	}

	public class FakedItem : Item, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedItem(ETiles _tile)
		{
			m_tile = _tile;
		}

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override ETiles Tile
		{
			get { return m_tile; }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return (Item)ThingHelper.ResolveThing(type, _creature);
		}
	}

	public class FakedMonster : Creature, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedMonster(ETiles _tile):base(Point.Zero, int.MinValue)
		{
			m_tile = _tile;
		}

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override ETiles Tile
		{
			get { return m_tile; }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return (Item)ThingHelper.ResolveThing(type, _creature);
		}
	}
}