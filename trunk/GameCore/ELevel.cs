using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore
{
    /// <summary>
    /// ������� ���������� ���������, ���� � �������. ������ ��������� 
    /// </summary>
    public enum ELevel
    {
        [Level("��������")]
        HEROVILL,
		
		[Level("�� ��������� ������")]
		WILDERNESS,
		
		[Level("���� ��������")]
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