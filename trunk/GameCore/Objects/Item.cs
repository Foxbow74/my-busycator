using System;
using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Misc;

namespace GameCore.Objects
{
	/// <summary>
	/// ��, ��� �������� ����� ����� � �������� � ���������
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
		[ItemCategory("������")]
		WEAR,
		[ItemCategory("�����")]
		ARMOR,
		[ItemCategory("������")]
		WEAPON,
		[ItemCategory("�������������")]
		JEVELRY,
		[ItemCategory("����")]
		FOOD,
		[ItemCategory("��������")]
		POTION,
		[ItemCategory("�������")]
		AMMO,
		[ItemCategory("�����������")]
		TOOLS,
		[ItemCategory("�����")]
		WANDS,
		[ItemCategory("�����")]
		BOOKS,
		[ItemCategory("������")]
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