using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Essences;

namespace GameCore.Creatures
{
	public enum EEquipmentPlaces
	{
		[EquipmentPlaces(EItemCategory.HELMETS)] HEAD,
		[EquipmentPlaces(EItemCategory.NECKLACES)] NECK,
		[EquipmentPlaces(EItemCategory.WEAR, EItemCategory.ARMOR)] BODY,
		[EquipmentPlaces(EItemCategory.GIRGLE)] GIRGLE,
		[EquipmentPlaces(EItemCategory.CLOACK)] CLOACK,
		[EquipmentPlaces] RIGHT_HAND,
		[EquipmentPlaces] LEFT_HAND,
		[EquipmentPlaces(EItemCategory.RINGS)] RIGHT_RING,
		[EquipmentPlaces(EItemCategory.RINGS)] LEFT_RING,
		[EquipmentPlaces(EItemCategory.BRACERS)] BRACERS,
		[EquipmentPlaces(EItemCategory.GAUNTLETS)] GAUNTLETS,
		[EquipmentPlaces(EItemCategory.BOOTS)] BOOTS,
		[EquipmentPlaces(EItemCategory.MISSILE_WEAPON)] MISSILE_WEAPON,
		[EquipmentPlaces] MISSILES,
		[EquipmentPlaces(EItemCategory.TOOLS)] TOOL,
	}


	public class EquipmentPlacesAttribute : Attribute
	{
		private static Dictionary<EEquipmentPlaces, EquipmentPlacesAttribute> m_attrs;

		public EquipmentPlacesAttribute(params EItemCategory[] _ableToEquip)
		{
			AbleToEquip = _ableToEquip;
		}

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