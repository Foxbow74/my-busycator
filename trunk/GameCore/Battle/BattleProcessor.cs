using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Essences;

namespace GameCore.Battle
{
	public class BattleProcessor
	{
		readonly Dictionary<Creature, CreatureBattleInfo> m_creatures = new Dictionary<Creature, CreatureBattleInfo>();
		readonly Dictionary<Item, ItemBattleInfo> m_items = new Dictionary<Item, ItemBattleInfo>();

		public CreatureBattleInfo this[Creature _creature]
		{
			get
			{
				CreatureBattleInfo info;
				if (!m_creatures.TryGetValue(_creature, out info))
				{
					info = _creature.CreateBattleInfo();
					m_creatures[_creature] = info;
				}
				return info;
			}
		}

		public ItemBattleInfo this[Creature _creature, Item _item]
		{
			get
			{
				ItemBattleInfo info;
				if (!m_items.TryGetValue(_item, out info))
				{
					info = _item.CreateItemInfo(_creature);
					m_items[_item] = info;
				}
				return info;
			}
		}
	}
}
