using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences.Weapons;
using GameCore.Messages;
using GameCore.XLanguage;

namespace GameCore.Battle
{
	public class CreatureBattleInfo
	{
		public CreatureBattleInfo(Creature _creature, int _dv, int _pv, int _toHit, int _dmg, Dice _hp):this()
		{
			Creature = _creature;
			DV = _dv;
			PV = _pv;
			HP = _hp.Calc();
			ToHitModifier = _toHit;
			DmgModifier = _dmg;
		}

		public CreatureBattleInfo(Creature _creature, int _dv, int _pv, Dice _hp)
			: this()
		{
			Creature = _creature;
			DV = _dv;
			PV = _pv;
			HP = _hp.Calc();
		}

		protected CreatureBattleInfo()
		{
			Agro = new Dictionary<Creature, int>();
		}

		public int HP { get; protected set; }

		public int DV { get; private set; }
		public int PV { get; private set; }

		public int ToHitModifier { get; protected set; }
		public int DmgModifier { get; private set; }

		public Dictionary<Creature, int> Agro { get; private set; }

		public Creature Creature { get; private set; }

		public void ApplyDamage(int _damage, IWeapon _weapon, Creature _source)
		{
			var fact = Math.Min(_damage, HP);

			if (!(Creature is Avatar))
			{
				HP -= _damage;
			}

			MessageManager.SendXMessage(this, new XMessage(EXMType.CREATURE_TAKES_DAMAGE, _source, Creature, fact, _weapon));

			fact -= Creature[0, 0].AddSplatter(fact, FColor.Crimson);
			if (fact > 0)
			{
				var ro = World.Rnd.NextDouble() * Math.PI * 2;
				var x = (int)(Math.Sin(ro) * 20f);
				var y = (int)(Math.Cos(ro) * 20f);

				new SplatterDropper(Creature.GeoInfo.Layer, Creature[0, 0], fact, FColor.Crimson, Creature[x, y]);
			}

			if (HP <= 0)
			{
				MessageManager.SendXMessage(this, new XMessage(EXMType.CREATURE_KILLED, _source, Creature));
				Creature[0, 0].AddItem(new Corpse(Creature));
				World.TheWorld.CreatureManager.CreatureIsDead(Creature);
			}
		}
	}
}