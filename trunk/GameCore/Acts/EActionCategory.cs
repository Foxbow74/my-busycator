using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Acts
{
	public enum EActionCategory
	{
		[ActionCategory("Перемещения")]
		MOVEMENT,
		[ActionCategory("Бой")]
		COMBAT,
		[ActionCategory("Работа с предметами")]
		ITEMS,
		[ActionCategory("Взаимодействие с миром")]
		WORLD_INTERACTIONS,
		[ActionCategory("Информация")]
		INFORMATION,
		[ActionCategory("Система")]
		SYSTEM
	}

	public class ActionCategoryAttribute : Attribute
	{
		private static Dictionary<EActionCategory, ActionCategoryAttribute> m_attrs;

		public ActionCategoryAttribute(string _displayName)
		{
			DisplayName = _displayName;
		}

		public string DisplayName { get; private set; }

		public static ActionCategoryAttribute GetAttribute(EActionCategory _enum)
		{
			if (m_attrs == null)
			{
				m_attrs = Util.Fill<EActionCategory, ActionCategoryAttribute>();
			}
			return m_attrs[_enum];
		}
	}
}