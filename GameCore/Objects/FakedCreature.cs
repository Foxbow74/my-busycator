﻿using System;
using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Objects
{
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

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }
	}
}