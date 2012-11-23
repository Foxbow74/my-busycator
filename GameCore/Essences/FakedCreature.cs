﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace GameCore.Essences
{
	public class FakedCreature : Creature, IFaked
	{
		private readonly ETileset m_tileset;
		private readonly List<Type> m_types = new List<Type>();

		public FakedCreature(ETileset _tileset)
			: base(null, int.MinValue) { m_tileset = _tileset; }

		public override string Name { get { throw new NotImplementedException(); } }

		public override EFraction Fraction
		{
			get { throw new NotImplementedException(); }
		}

		#region IFaked Members

		public override ETileset Tileset { get { return m_tileset; } }

		public Essence ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			var thing = (Essence) Activator.CreateInstance(type, _creature.Layer);
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

		internal override CreatureBattleInfo CreateBattleInfo()
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			throw new NotImplementedException();
		}

		public override bool Is<T>()
		{
			return m_types.Any(_type => typeof(T).IsAssignableFrom(_type));
		}
	}
}