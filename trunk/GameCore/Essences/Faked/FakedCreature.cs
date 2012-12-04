using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace GameCore.Essences.Faked
{
	public class FakedCreature : Creature, IFaked
	{
		private readonly Essence m_essence;

		public FakedCreature(Essence _essence)
			: base(_essence.Name, null, 0)
		{
			TileIndex = _essence.TileIndex;
			m_essence = _essence;
		}

		public override FColor LerpColor { get { return Essence.LerpColor; } }

		public override Noun Name { get { throw new NotImplementedException(); } }

		public override EFraction Fraction
		{
			get { throw new NotImplementedException(); }
		}

		#region IFaked Members

		public override ETileset Tileset { get { return m_essence.Tileset; } }

		public Essence Essence
		{
			get { return m_essence; }
		}

		#endregion

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }

		public override CreatureBattleInfo CreateBattleInfo()
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			throw new NotImplementedException();
		}

		public override bool Is<T>()
		{
			return m_essence is T;
		}
	}
}