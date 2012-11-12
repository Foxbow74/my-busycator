using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
    /// <summary>
    /// Уровень объединяет местность, вещи и существ. Должен указывать 
    /// </summary>
    public enum ELevel
    {
        [Level("Херовиль")]
        HEROVILL,
		
		[Level("За городской стеной")]
		WILDERNESS,
		
		[Level("Даль страшная")]
        DANGERNESS,
    }

    public class LevelAttribute : Attribute
    {
        private static Dictionary<ELevel, LevelAttribute> m_attrs;

        public LevelAttribute(string _displayName)
        {
            DisplayName = _displayName;
        }

        public string DisplayName { get; private set; }

        public static LevelAttribute GetAttribute(ELevel _enum)
        {
            if (m_attrs == null)
            {
                m_attrs = Util.Fill<ELevel, LevelAttribute>();
            }
            return m_attrs[_enum];
        }
    }
}