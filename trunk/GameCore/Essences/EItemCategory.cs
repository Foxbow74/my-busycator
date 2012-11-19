using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Essences
{
    public enum EItemCategory
    {
        [EssenceCategory("-", ' ', ConsoleKey.NoName, EKeyModifiers.NONE)]
        NONE,
        [EssenceCategory("Шлемы", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        HELMETS,
        [EssenceCategory("Ожерелья", '\'', ConsoleKey.Oem7, EKeyModifiers.NONE)]
        NECKLACES,
        [EssenceCategory("Одежда", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        WEAR,
        [EssenceCategory("Броня", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        ARMOR,
        [EssenceCategory("Пояса", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        GIRGLE,
        [EssenceCategory("Плащи", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        CLOACK,
        [EssenceCategory("Наручи", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        BRACERS,
        [EssenceCategory("Перчатки", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        GAUNTLETS,
        [EssenceCategory("Обувь", '[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        BOOTS,
        [EssenceCategory("Оружие", '(', ConsoleKey.D9, EKeyModifiers.SHIFT)]
        WEAPON,
        [EssenceCategory("Метательное оружие", '}', ConsoleKey.Oem6, EKeyModifiers.SHIFT)]
        MISSILE_WEAPON,

        [EssenceCategory("Кольца", '=', ConsoleKey.OemPlus, EKeyModifiers.NONE)]
        RINGS,
        [EssenceCategory("Пища", '%', ConsoleKey.D5, EKeyModifiers.SHIFT)]
        FOOD,
        [EssenceCategory("Снадобья", '!', ConsoleKey.D1, EKeyModifiers.SHIFT)]
        POTION,
        [EssenceCategory("Снаряды", '/', ConsoleKey.Oem2, EKeyModifiers.NONE)]
        MISSILES,
        [EssenceCategory("Инструменты", ']', ConsoleKey.Oem6, EKeyModifiers.NONE)]
        TOOLS,
        [EssenceCategory("Жезлы", '\\', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        WANDS,
        [EssenceCategory("Книги", '"', ConsoleKey.D2, EKeyModifiers.SHIFT)]
        BOOKS,
        [EssenceCategory("Свитки", '?', ConsoleKey.Oem2, EKeyModifiers.SHIFT)]
        SCROLLS,
    }

    public class EssenceCategoryAttribute : Attribute
    {
        private static Dictionary<EItemCategory, EssenceCategoryAttribute> m_attrs;

        public EssenceCategoryAttribute(string _displayName, char _char, ConsoleKey _key, EKeyModifiers _modifiers)
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

        public static EssenceCategoryAttribute GetAttribute(EItemCategory _enum)
        {
            if (m_attrs == null)
            {
                m_attrs = Util.Fill<EItemCategory, EssenceCategoryAttribute>();
            }
            return m_attrs[_enum];
        }
    }
}