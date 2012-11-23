using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Battle
{
	public class CreatureBattleInfo
	{
		private readonly Creature m_creature;

		public CreatureBattleInfo(Creature _creature, int _dv, int _pv, int _toHit, int _dmg, Dice _hp):this()
		{
			m_creature = _creature;
			DV = _dv;
			PV = _pv;
			HP = _hp.Calc();
			ToHitModifier = _toHit;
			DmgModifier = _dmg;
		}

		public CreatureBattleInfo(Creature _creature, int _dv, int _pv, Dice _hp)
			: this()
		{
			m_creature = _creature;
			DV = _dv;
			PV = _pv;
			HP = _hp.Calc();
		}

		protected CreatureBattleInfo()
		{
			Agro = new Dictionary<Creature, int>();
		}

		public int HP { get; private set; }

		public int DV { get; private set; }
		public int PV { get; private set; }

		public int ToHitModifier { get; private set; }
		public int DmgModifier { get; private set; }

		public Dictionary<Creature, int> Agro { get; private set; }
	}
}