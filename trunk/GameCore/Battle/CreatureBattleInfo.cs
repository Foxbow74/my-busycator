using System.Collections.Generic;
using GameCore.Creatures;

namespace GameCore.Battle
{
	public class CreatureBattleInfo
	{
		private readonly Creature m_creature;

		public CreatureBattleInfo(Creature _creature, int _dv, int _pv, int _toHit, Dice _dmg, Dice _hp):this()
		{
			m_creature = _creature;
			DV = _dv;
			PV = _pv;
			ToHit = _toHit;
			Dmg = _dmg;

			HP = _hp.Calc();
		}

		protected CreatureBattleInfo()
		{
			Agro = new Dictionary<Creature, int>();
		}

		public int HP { get; private set; }

		public int DV { get; private set; }
		public int PV { get; private set; }

		public int ToHit { get; private set; }
		public Dice Dmg { get; private set; }

		public Dictionary<Creature, int> Agro { get; private set; }
	}
}