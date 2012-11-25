using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using GameCore.Messages;
using GameCore.Misc;
using RusLanguage;

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
			//if (_creature is Missile)
			//{
			//    var missile = (Missile) _creature;
			//    MessageManager.SendMessage(this, missile[EPadej.IMEN] + " угодил прямо в " + _target[EPadej.ROD]);
				
			//    _creature.LiveCoords = null;
			//    return EActResults.DONE;
			//}
			if(_creature is SplatterDropper)
			{
				_creature.LiveCoords = null;
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
						MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Чудом вы попали по " + _target[EPadej.DAT]));
					}
					else
					{
						MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Вы попали по " + _target[EPadej.DAT]));
					}
				}
				else if(_target is Avatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature[EPadej.IMEN] + " дотянулся до вас"));
				}

				var pv = targetBattleInfo.PV;
				var damage = itemBattleInfo.Dmg.Calc();
				var isCritical = itemBattleInfo.Dmg.Max == damage;
				damage += creatureBattleInfo.DmgModifier;

				if (isCritical && _creature.IsAvatar)
				{
					MessageManager.SendMessage(this, "отличный удар");
					damage *= 2;
				}

				damage -= pv;

				if(damage>0)
				{
					targetBattleInfo.ApplyDamage(damage, weapon);
				}
				else
				{
					if (_creature.IsAvatar)
					{
						MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "но силы удара оказалось недостаточно"));
					}
					else
					{
						MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "но лишь оцарапал броню"));
					}
				}
			}
			else
			{
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Удар не попал по " + _target[EPadej.DAT]));
				}
				else
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature[EPadej.IMEN] + " промахнулся"));
				}
			}

			if (_creature is Missile)
			{
				_creature.LiveCoords = null;
			}
			return EActResults.DONE;
			
		}
	}
}
