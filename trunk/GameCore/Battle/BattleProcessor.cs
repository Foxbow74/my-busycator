using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Battle
{
	public class BattleProcessor
	{
		readonly Dictionary<Creature, CreatureBattleInfo> m_creatures = new Dictionary<Creature, CreatureBattleInfo>();
		readonly Dictionary<Item, ItemBattleInfo> m_items = new Dictionary<Item, ItemBattleInfo>();

		public CreatureBattleInfo this[Creature _creature, bool _try = false]
		{
			get
			{
				CreatureBattleInfo info;
				if (!m_creatures.TryGetValue(_creature, out info) && !_try)
				{
					info = _creature.CreateBattleInfo();
					m_creatures[_creature] = info;
				}
				return info;
			}
		}

		public ItemBattleInfo this[Item _item, Creature _creature, bool _try = false]
		{
			get
			{
				ItemBattleInfo info;
				if (!m_items.TryGetValue(_item, out info) && !_try)
				{
					info = _item.CreateItemInfo(_creature);
					m_items[_item] = info;
				}
				return info;
			}
		}

		public EActResults Atack(Creature _creature, Creature _target)
		{
			var weapons = _creature.GetWeapons(_target).ToArray();
			if(_creature is SplatterDropper)
			{
				World.TheWorld.CreatureManager.CreatureIsDead(_creature);
				return EActResults.DONE;
			}

			var weapon = weapons.RandomItem(World.Rnd);
			var itemBattleInfo = this[(Item)weapon, _creature];

			var creatureBattleInfo = this[_creature];
			var targetBattleInfo = this[_target];

			var toHit = creatureBattleInfo.ToHitModifier + itemBattleInfo.ToHit + 1;
			var dv = targetBattleInfo.DV + 1;

			var dvVsToHit = World.Rnd.Next(toHit + dv) - dv;

			var isLuck = false;
			if (dvVsToHit <= 0 && _creature is Avatar)
			{
				dvVsToHit += World.Rnd.Next(World.TheWorld.Avatar.LuckModifier + 1);
				isLuck = true;
			}

			if (dvVsToHit > 0)
			{
				if (_creature.IsAvatar)
				{
					if (isLuck)
					{
						MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.AVATAR_IS_LUCK, _creature));
					}
				}
				MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, _creature, _target, weapon));

				var damage = itemBattleInfo.Dmg.Roll();
				var isCritical = itemBattleInfo.Dmg.Max == damage;
				damage += creatureBattleInfo.DmgModifier;
				if (damage == 0)
				{
					MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO, _creature, _target, weapon));
				}
				else
				{
					if (isCritical && _creature.IsAvatar)
					{
						MessageManager.SendMessage(this, "отличный удар");
						damage *= 2;
					}


					var pv = targetBattleInfo.PV;
					damage -= pv;
					if (damage > 0)
					{
						targetBattleInfo.ApplyDamage(damage, weapon, _creature);
					}
					else
					{
						MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, _creature, _target, weapon));
					}
				}
			}
			else
			{
				MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, _creature, _target, weapon));
			}

			if (_creature is Missile)
			{
				World.TheWorld.CreatureManager.CreatureIsDead(_creature);
			}
			return EActResults.DONE;
			
		}
	}
}
