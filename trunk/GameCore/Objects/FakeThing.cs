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

		public FakedFurniture(ETiles _tile, Material _material):base(_material)
		{
			m_tile = _tile;
		}

		public override ETiles Tile
		{
			get { return m_tile; }
		}

		public override string Name
		{
			get
			{
				try
				{
					throw new NotImplementedException();
				}
				catch 
				{
				}
				return "���!";
			}
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

		public override bool Is<T>() 
		{
			return m_types.All(_type => typeof (T).IsAssignableFrom(_type));
		}

		public override EMaterial AllowedMaterials
		{
			get { throw new NotImplementedException(); }
		}

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}

	public class FakedMonster : Creature, IFaked
	{
		private readonly ETiles m_tile;
		private readonly List<Type> m_types = new List<Type>();

		public FakedMonster(ETiles _tile)
			: base(null, int.MinValue)
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

		public override EThinkingResult Thinking()
		{
			throw new NotImplementedException();
		}
	}
}