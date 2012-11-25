using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences.Weapons;
using GameCore.Messages;
using GameCore.Misc;
using RusLanguage;

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

		public int HP { get; protected set; }

		public int DV { get; private set; }
		public int PV { get; private set; }

		public int ToHitModifier { get; protected set; }
		public int DmgModifier { get; private set; }

		public Dictionary<Creature, int> Agro { get; private set; }

		public void ApplyDamage(int _damage, IWeapon _weapon)
		{
			var fact = Math.Min(_damage, HP);

			Debug.WriteLine(m_creature[EPadej.IMEN] + " hp=" + HP + " dmg=" + _damage);
			if (!(m_creature is Avatar))
			{
				HP -= _damage;
			}
			if(HP<=0)
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, m_creature[EPadej.IMEN] + " погиб"));
				m_creature[0, 0].AddItem(new Corpse(m_creature));
				m_creature[0, 0].AddSplatter(new Splatter(FColor.Crimson, Splatter.COUNT - World.Rnd.Next(fact % Splatter.COUNT)));
				m_creature.LiveCoords = null;
			}
			else
			{
				if (m_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "получено " + _damage.Пунктов() + " урона"));
				}
				else
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, m_creature[EPadej.IMEN] + " получил " + _damage.Пунктов() + " урона"));
				}

				var ro = World.Rnd.NextDouble()*Math.PI*2;
				var x = (int)(Math.Sin(ro) * 20f);
				var y = (int)(Math.Cos(ro) * 20f);

				new SplatterDropper(m_creature.Layer, m_creature.LiveCoords, fact, FColor.Crimson, m_creature[x, y].LiveCoords);
				///m_creature[0,0].AddCreature();

				//var pnt = Point.Zero.NearestPoints.ToArray().RandomItem(World.Rnd);
				//m_creature[pnt.X, pnt.Y].AddSplatter(new Splatter(FColor.Crimson, World.Rnd.Next(Math.Abs(10 - _damage))));
			}
		}
	}
}