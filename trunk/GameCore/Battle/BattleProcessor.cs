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
			if (_creature is Missile)
			{
				MessageManager.SendMessage(this, "попал!");
				_creature.LiveCoords = null;
				return EActResults.DONE;
			}
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
			if(World.Rnd.Next(toHit + dv)<toHit)
			{
				var pv = targetBattleInfo.PV;
				var damage = creatureBattleInfo.DmgModifier + itemBattleInfo.Dmg.Calc();

				damage -= pv;

				if(damage>0)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _target[EPadej.IMEN] + " огреб " + damage +" пунктов урона"));
				}
				else
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Урон был поглощен броней " + _target[EPadej.ROD]));
				}
			}
			else
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "Удар не попал по " + _target[EPadej.DAT]));
			}
			return EActResults.DONE;
			
		}
	}
}
