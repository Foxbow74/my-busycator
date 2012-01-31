﻿using System;
using System.Collections.Generic;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	public enum EEquipmentPlaces
	{
		[EquipmentPlaces("Голова", EThingCategory.HELMETS)]
		HEAD,
		[EquipmentPlaces("Шея", EThingCategory.NECKLACES)]
		NECK,
		[EquipmentPlaces("Тело", EThingCategory.WEAR, EThingCategory.ARMOR)]
		BODY,
		[EquipmentPlaces("Пояс", EThingCategory.GIRGLE)]
		GIRGLE,
		[EquipmentPlaces("Накидка", EThingCategory.CLOACK)]
		CLOACK,
		[EquipmentPlaces("Правая рука")]
		RIGHT_HAND,
		[EquipmentPlaces("Левая рука")]
		LEFT_HAND,
		[EquipmentPlaces("Кольцо на правой руке", EThingCategory.RINGS)]
		RIGHT_RING,
		[EquipmentPlaces("Кольцо на левой руке", EThingCategory.RINGS)]
		LEFT_RING,
		[EquipmentPlaces("Наручи", EThingCategory.BRACERS)]
		BRACERS,
		[EquipmentPlaces("Перчатки", EThingCategory.GAUNTLETS)]
		GAUNTLETS,
		[EquipmentPlaces("Обувь", EThingCategory.BOOTS)]
		BOOTS,
		[EquipmentPlaces("Метательное оружие", EThingCategory.MISSILE_WEAPON)]
		MISSILE_WEAPON,
		[EquipmentPlaces("Снаряды", EThingCategory.MISSILES)]
		MISSILES,
		[EquipmentPlaces("Инструмент", EThingCategory.TOOLS)]
		TOOL,
	}


	public class EquipmentPlacesAttribute : Attribute
	{
		private static Dictionary<EEquipmentPlaces, EquipmentPlacesAttribute> m_attrs;

		public EquipmentPlacesAttribute(string _displayName, params EThingCategory[] _ableToEquip)
		{
			DisplayName = _displayName;
			AbleToEquip = _ableToEquip;
		}

		public string DisplayName { get; private set; }

		public EThingCategory[] AbleToEquip { get; set; }

		public static EquipmentPlacesAttribute GetAttribute(EEquipmentPlaces _enum)
		{
			return Attrs[_enum];
		}

		public static IEnumerable<EEquipmentPlaces> AllValues{get{return Attrs.Keys;}}

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
	}
}
