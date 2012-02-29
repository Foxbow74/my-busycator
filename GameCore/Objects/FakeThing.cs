using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public class FakedThing : Thing, IFaked
	{
		private readonly ETiles m_tile;
		private readonly FColor m_lerpColor;
		private readonly List<Type> m_types = new List<Type>();

		public FakedThing(ETiles _tile, FColor _lerpColor)
		{
			m_tile = _tile;
			m_lerpColor = _lerpColor;
		}

		public override FColor LerpColor
		{
			get
			{
				return m_lerpColor;
			}
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
				return "¡Àﬂ!";
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
			return ThingHelper.ResolveThing(type, _creature);
		}

		#endregion

		public override bool Is<T>() 
		{
			return m_types.All(_type => typeof (T).IsAssignableFrom(_type));
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
		private readonly FColor m_lerpColor;
		private readonly List<Type> m_types = new List<Type>();

		public FakedMonster(ETiles _tile, FColor _lerpColor)
			: base(null, int.MinValue)
		{
			m_tile = _tile;
			m_lerpColor = _lerpColor;
		}

		public override FColor LerpColor
		{
			get
			{
				return m_lerpColor;
			}
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
			return ThingHelper.ResolveThing(type, _creature);
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
	}
}