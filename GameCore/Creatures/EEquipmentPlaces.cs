using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;
using GameCore.Essences;

namespace GameCore.Creatures
{
	public enum EEquipmentPlaces
	{
		[EquipmentPlaces("Голова", EEssenceCategory.HELMETS)] HEAD,
		[EquipmentPlaces("Шея", EEssenceCategory.NECKLACES)] NECK,
		[EquipmentPlaces("Тело", EEssenceCategory.WEAR, EEssenceCategory.ARMOR)] BODY,
		[EquipmentPlaces("Пояс", EEssenceCategory.GIRGLE)] GIRGLE,
		[EquipmentPlaces("Накидка", EEssenceCategory.CLOACK)] CLOACK,
		[EquipmentPlaces("Правая рука")] RIGHT_HAND,
		[EquipmentPlaces("Левая рука")] LEFT_HAND,
		[EquipmentPlaces("Кольцо на правой руке", EEssenceCategory.RINGS)] RIGHT_RING,
		[EquipmentPlaces("Кольцо на левой руке", EEssenceCategory.RINGS)] LEFT_RING,
		[EquipmentPlaces("Наручи", EEssenceCategory.BRACERS)] BRACERS,
		[EquipmentPlaces("Перчатки", EEssenceCategory.GAUNTLETS)] GAUNTLETS,
		[EquipmentPlaces("Обувь", EEssenceCategory.BOOTS)] BOOTS,
		[EquipmentPlaces("Метательное оружие", EEssenceCategory.MISSILE_WEAPON)] MISSILE_WEAPON,
		[EquipmentPlaces("Снаряды")] MISSILES,
		[EquipmentPlaces("Инструмент", EEssenceCategory.TOOLS)] TOOL,
	}


	public class EquipmentPlacesAttribute : Attribute
	{
		private static Dictionary<EEquipmentPlaces, EquipmentPlacesAttribute> m_attrs;

		public EquipmentPlacesAttribute(string _displayName, params EEssenceCategory[] _ableToEquip)
		{
			DisplayName = _displayName;
			AbleToEquip = _ableToEquip;
		}

		public string DisplayName { get; private set; }

		public EEssenceCategory[] AbleToEquip { get; set; }

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

		public bool IsAbleToEquip(EEssenceCategory _category) { return AbleToEquip.Length == 0 || AbleToEquip.Contains(_category); }

		public static EquipmentPlacesAttribute GetAttribute(EEquipmentPlaces _enum) { return Attrs[_enum]; }
	}
}