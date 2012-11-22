using System;
using System.Collections.Generic;
using System.Globalization;
using GameCore.Creatures;
using GameCore.Essences;

namespace GameCore.Battle
{
	public class BattleProcessor
	{
		readonly Dictionary<Creature, CreatureInfo> m_creatures = new Dictionary<Creature, CreatureInfo>();
		readonly Dictionary<Item, ItemInfo> m_items = new Dictionary<Item, ItemInfo>();

		public CreatureInfo this[Creature _creature]
		{
			get
			{
				CreatureInfo info;
				if (!m_creatures.TryGetValue(_creature, out info))
				{
					info = _creature.CreateBattleInfo();
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
					info = _item.CreateItemInfo(_creature);
					m_items[_item] = info;
				}
				return info;
			}
		}
	}

	public class ItemInfo
	{
		public int DV { get; set; }
		public int PV { get; set; }
		public int PVI { get; set; }

		public int ToHit { get; set; }
		public Dice Dmg { get; set; }
	}

	public class CreatureInfo
	{
		public CreatureInfo(int _dv, int _pv, int _toHit, Dice _dmg)
		{
			Agro = new Dictionary<Creature, int>();
		}

		public int DV { get; private set; }
		public int PV { get; private set; }

		public int ToHit { get; private set; }
		public Dice Dmg { get; private set; }

		public Dictionary<Creature, int> Agro { get; private set; }
	}

	public enum EFraction
	{
		DUMMY,
		AVATAR,
		MONSTERS,
	}

	/// <summary>
	/// 2d8+1
	/// </summary>
	public class Dice
	{
		public int Count { get; set; }
		public int Size { get; set; }
		public int Modifier { get; set; }

		public override string ToString()
		{
			return string.Format("{0}d{1}{2}", Count, Size,(Modifier == 0? "": (Modifier > 0 ? ("+" + Modifier) : Modifier.ToString(CultureInfo.InvariantCulture))));
		}

		public int Calc()
		{
			var result = Modifier;
			for (var i = 0; i < Count; i++)
			{
				result += World.Rnd.Next(Size);
			}
			return result;
		}
	}
}
