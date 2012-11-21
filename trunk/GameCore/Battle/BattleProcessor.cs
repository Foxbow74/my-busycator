using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Essences;

namespace GameCore.Battle
{
	class BattleProcessor
	{
		Dictionary<Creature, CreatureInfo> m_creatures = new Dictionary<Creature, CreatureInfo>();
		Dictionary<Item, ItemInfo> m_items = new Dictionary<Item, ItemInfo>();

		public CreatureInfo this[Creature _creature]
		{
			get
			{
				CreatureInfo info;
				if (!m_creatures.TryGetValue(_creature, out info))
				{
					info = Resolve(_creature);
					m_creatures[_creature] = info;
				}
				return info;
			}
		}

		public ItemInfo this[Creature _creature, Item _item]
		{
			get
			{
				ItemInfo info;
				if (!m_items.TryGetValue(_item, out info))
				{
					info = Resolve(_creature, _item);
					m_items[_item] = info;
				}
				return info;
			}
		}

		private ItemInfo Resolve(Creature _creature, Item _item)
		{
			throw new NotImplementedException();
		}

		private CreatureInfo Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}

	class ItemInfo
	{
		public int DV { get; set; }
		public int PV { get; set; }
		public int PVI { get; set; }

		public int ToHit { get; set; }
		public Dice Dmg { get; set; }
	}

	class CreatureInfo
	{
		public int DV { get; set; }
		public int PV { get; set; }

		public int ToHit { get; set; }
		public Dice Dmg { get; set; }
	}

	/// <summary>
	/// 2d8+1
	/// </summary>
	class Dice
	{
		public int Count { get; set; }
		public int Size { get; set; }
		public int Modifier { get; set; }

		public override string ToString()
		{
			//return string.Format("{0}d{1}{2}", Count, Size, (Modifier == 0 ? "" : (Modifier > 0 ? ("+" + Modifier) : Modifier.ToString(CultureInfo.InvariantCulture))));
			return string.Format(Math.Max(0, Count + Modifier) + " - " + Math.Max(0, Count*Size + Modifier));
		}
	}
}
