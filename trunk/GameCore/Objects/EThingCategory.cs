using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Objects
{
	public enum EThingCategory
	{
		[ThingCategory("Шлемы", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		HELMETS,
		[ThingCategory("Ожерелья", '\'', ConsoleKey.Oem7, EKeyModifiers.NONE)]
		NECKLACES,
		[ThingCategory("Одежда", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		WEAR,
		[ThingCategory("Броня", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		ARMOR,
		[ThingCategory("Пояса", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		GIRGLE,
		[ThingCategory("Плащи", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		CLOACK,
		[ThingCategory("Наручи", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		BRACERS,
		[ThingCategory("Перчатки", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		GAUNTLETS,
		[ThingCategory("Обувь", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		BOOTS,
		[ThingCategory("Оружие", '(', ConsoleKey.D9, EKeyModifiers.SHIFT)]
		WEAPON,
		[ThingCategory("Метательное оружие", '}', ConsoleKey.Oem6, EKeyModifiers.SHIFT)]
		MISSILE_WEAPON,

		[ThingCategory("Кольца", '=', ConsoleKey.OemPlus, EKeyModifiers.NONE)]
		RINGS,
		[ThingCategory("Пища", '%', ConsoleKey.D5, EKeyModifiers.SHIFT)]
		FOOD,
		[ThingCategory("Снадобья", '!', ConsoleKey.D1, EKeyModifiers.SHIFT)]
		POTION,
		[ThingCategory("Снаряды", '/', ConsoleKey.Oem2, EKeyModifiers.NONE)]
		MISSILES,
		[ThingCategory("Инструменты", ']', ConsoleKey.Oem6, EKeyModifiers.NONE)]
		TOOLS,
		[ThingCategory("Жезлы", '\\', ConsoleKey.Oem5, EKeyModifiers.NONE)]
		WANDS,
		[ThingCategory("Книги", '"', ConsoleKey.D2, EKeyModifiers.SHIFT)]
		BOOKS,
		[ThingCategory("Свитки", '?', ConsoleKey.Oem2, EKeyModifiers.SHIFT)]
		SCROLLS,
		[ThingCategory("Мебель", '#', ConsoleKey.D3, EKeyModifiers.SHIFT)]
		FURNITURE,
	}

	public class ThingCategoryAttribute : Attribute
	{
		private static Dictionary<EThingCategory, ThingCategoryAttribute> m_attrs;

		public ThingCategoryAttribute(string _displayName, char _char, ConsoleKey _key, EKeyModifiers _modifiers)
		{
			DisplayName = _displayName;
			C = _char;
			Key = _key;
			Modifiers = _modifiers;
		}

		public string DisplayName { get; private set; }
		public char C { get; private set; }
		public ConsoleKey Key { get; set; }
		public EKeyModifiers Modifiers { get; set; }

		public static ThingCategoryAttribute GetAttribute(EThingCategory _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<EThingCategory, ThingCategoryAttribute>();
			}
			return m_attrs[_enum];
		}
	}
}