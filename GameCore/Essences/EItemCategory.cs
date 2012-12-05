using System;
using System.Collections.Generic;

namespace GameCore.Essences
{
    public enum EItemCategory
    {
        [EssenceCategory(' ', ConsoleKey.NoName, EKeyModifiers.NONE)]
        NONE,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        HELMETS,
        [EssenceCategory('\'', ConsoleKey.Oem7, EKeyModifiers.NONE)]
        NECKLACES,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        WEAR,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        ARMOR,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        GIRGLE,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        CLOACK,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        BRACERS,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        GAUNTLETS,
        [EssenceCategory('[', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        BOOTS,
        [EssenceCategory('(', ConsoleKey.D9, EKeyModifiers.SHIFT)]
        WEAPON,
        [EssenceCategory('}', ConsoleKey.Oem6, EKeyModifiers.SHIFT)]
        MISSILE_WEAPON,
        [EssenceCategory('=', ConsoleKey.OemPlus, EKeyModifiers.NONE)]
        RINGS,
        [EssenceCategory('%', ConsoleKey.D5, EKeyModifiers.SHIFT)]
        FOOD,
        [EssenceCategory('!', ConsoleKey.D1, EKeyModifiers.SHIFT)]
        POTION,
        [EssenceCategory('/', ConsoleKey.Oem2, EKeyModifiers.NONE)]
        MISSILES,
        [EssenceCategory(']', ConsoleKey.Oem6, EKeyModifiers.NONE)]
        TOOLS,
        [EssenceCategory('\\', ConsoleKey.Oem5, EKeyModifiers.NONE)]
        WANDS,
        [EssenceCategory('"', ConsoleKey.D2, EKeyModifiers.SHIFT)]
        BOOKS,
        [EssenceCategory('?', ConsoleKey.Oem2, EKeyModifiers.SHIFT)]
        SCROLLS,
    }

    public class EssenceCategoryAttribute : Attribute
    {
        private static Dictionary<EItemCategory, EssenceCategoryAttribute> m_attrs;

        public EssenceCategoryAttribute(char _char, ConsoleKey _key, EKeyModifiers _modifiers)
        {
            C = _char;
            Key = _key;
            Modifiers = _modifiers;
        }

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