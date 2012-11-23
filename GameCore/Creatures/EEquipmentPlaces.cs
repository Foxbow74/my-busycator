using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Essences;
using GameCore.Misc;

namespace GameCore.Creatures
{
	public enum EEquipmentPlaces
	{
		[EquipmentPlaces("Голова", EItemCategory.HELMETS)] HEAD,
		[EquipmentPlaces("Шея", EItemCategory.NECKLACES)] NECK,
		[EquipmentPlaces("Тело", EItemCategory.WEAR, EItemCategory.ARMOR)] BODY,
		[EquipmentPlaces("Пояс", EItemCategory.GIRGLE)] GIRGLE,
		[EquipmentPlaces("Накидка", EItemCategory.CLOACK)] CLOACK,
		[EquipmentPlaces("Правая рука")] RIGHT_HAND,
		[EquipmentPlaces("Левая рука")] LEFT_HAND,
		[EquipmentPlaces("Кольцо на правой руке", EItemCategory.RINGS)] RIGHT_RING,
		[EquipmentPlaces("Кольцо на левой руке", EItemCategory.RINGS)] LEFT_RING,
		[EquipmentPlaces("Наручи", EItemCategory.BRACERS)] BRACERS,
		[EquipmentPlaces("Перчатки", EItemCategory.GAUNTLETS)] GAUNTLETS,
		[EquipmentPlaces("Обувь", EItemCategory.BOOTS)] BOOTS,
		[EquipmentPlaces("Метательное оружие", EItemCategory.MISSILE_WEAPON)] MISSILE_WEAPON,
		[EquipmentPlaces("Снаряды")] MISSILES,
		[EquipmentPlaces("Инструмент", EItemCategory.TOOLS)] TOOL,
	}


	public class EquipmentPlacesAttribute : Attribute
	{
		private static Dictionary<EEquipmentPlaces, EquipmentPlacesAttribute> m_attrs;

		public EquipmentPlacesAttribute(string _displayName, params EItemCategory[] _ableToEquip)
		{
			DisplayName = _displayName;
			AbleToEquip = _ableToEquip;
		}

		public string DisplayName { get; private set; }

		public EItemCategory[] AbleToEquip { get; set; }

		public static IEnumerable<EEquipmentPlaces> AllValues { get { return Attrs.Keys; } }

		public static Dictionary<EEquipmentPlaces, EquipmentPlacesAttribute> Attrs
		{
			get
			{
				if (m_attrs == null)
				{
					m_attrs = Util.Fill<EEquipmentPlaces, EquipmentPlacesAttribute>();
				}
				return m_attrs;
			}
		}

		public bool IsAbleToEquip(EItemCategory _category) { return AbleToEquip.Length == 0 || AbleToEquip.Contains(_category); }

		public static EquipmentPlacesAttribute GetAttribute(EEquipmentPlaces _enum) { return Attrs[_enum]; }
	}
}