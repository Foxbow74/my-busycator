using System;
using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Misc;

namespace GameCore.Objects
{
	/// <summary>
	/// То, что существо может взять и положить в инвентарь
	/// </summary>
	public abstract class Item : Thing
	{
		public override int GetHashCode()
		{
			return CalcHashCode();
		}

		protected int CalcHashCode()
		{
			return GetType().GetHashCode();
		}

		public override float Opaque
		{
			get
			{
				return 0;
			}
		}

		public abstract EItemCategory Category { get; }
	}

	public enum EItemCategory
	{
		[ItemCategory("Одежда")]
		WEAR,
		[ItemCategory("Броня")]
		ARMOR,
		[ItemCategory("Оружие")]
		WEAPON,
		[ItemCategory("Драгоценности")]
		JEVELRY,
		[ItemCategory("Пища")]
		FOOD,
		[ItemCategory("Снадобья")]
		POTION,
		[ItemCategory("Снаряды")]
		AMMO,
		[ItemCategory("Инструменты")]
		TOOLS,
		[ItemCategory("Жезлы")]
		WANDS,
		[ItemCategory("Книги")]
		BOOKS,
		[ItemCategory("Свитки")]
		SCROLLS,
	}


	public class ItemCategoryAttribute : Attribute
	{
		private static Dictionary<EItemCategory, ItemCategoryAttribute> m_attrs;

		public ItemCategoryAttribute(string _displayName)
		{
			DisplayName = _displayName;
		}

		public string DisplayName { get; private set; }

		public static ItemCategoryAttribute GetAttribute(EItemCategory _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<EItemCategory, ItemCategoryAttribute>();
			}
			return m_attrs[_enum];
		}
	}
}