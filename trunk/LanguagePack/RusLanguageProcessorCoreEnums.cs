using System;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Essences;

namespace LanguagePack
{
	public partial class RusLanguageProcessor
	{
		public Noun AsNoun(EEquipmentPlaces _e)
		{
			switch (_e)
			{
				case EEquipmentPlaces.HEAD:
					return "������".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.NECK:
					return "���".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.BODY:
					return "����".AsNoun(ESex.IT, false);
				case EEquipmentPlaces.GIRGLE:
					return "����".AsNoun(ESex.MALE, false);
				case EEquipmentPlaces.CLOACK:
					return "�������".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.RIGHT_HAND:
					return "����".AsNoun(ESex.FEMALE, false) + "������".AsAdj();
				case EEquipmentPlaces.LEFT_HAND:
					return "����".AsNoun(ESex.FEMALE, false) + "�����".AsAdj();
				case EEquipmentPlaces.RIGHT_RING:
					return "������".AsNoun(ESex.IT, false) + "�� ������ ����".AsIm();
				case EEquipmentPlaces.LEFT_RING:
					return "������".AsNoun(ESex.IT, false) + "�� ����� ����".AsIm();
				case EEquipmentPlaces.BRACERS:
					return "������".AsNoun(ESex.PLURAL, false);
				case EEquipmentPlaces.GAUNTLETS:
					return "��������".AsNoun(ESex.PLURAL, false);
				case EEquipmentPlaces.BOOTS:
					return "�����".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.MISSILE_WEAPON:
					return "������".AsNoun(ESex.IT, false) + "�����������".AsAdj();
				case EEquipmentPlaces.MISSILES:
					return "�������".AsNoun(ESex.PLURAL, false);
				case EEquipmentPlaces.TOOL:
					return "����������".AsNoun(ESex.MALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(EItemCategory _e)
		{
			switch (_e)
			{
				case EItemCategory.NONE:
					return "-".AsNoun(ESex.MALE, false);
				case EItemCategory.HELMETS:
					return "����".AsNoun(ESex.MALE, false);
				case EItemCategory.NECKLACES:
					return "��������".AsNoun(ESex.IT, false);
				case EItemCategory.WEAR:
					return "������".AsNoun(ESex.FEMALE, false);
				case EItemCategory.ARMOR:
					return "�����".AsNoun(ESex.FEMALE, false);
				case EItemCategory.GIRGLE:
					return "����".AsNoun(ESex.MALE, false);
				case EItemCategory.CLOACK:
					return "����".AsNoun(ESex.MALE, false);
				case EItemCategory.BRACERS:
					return "������".AsNoun(ESex.PLURAL, false);
				case EItemCategory.GAUNTLETS:
					return "��������".AsNoun(ESex.PLURAL_FEMALE, false);
				case EItemCategory.BOOTS:
					return "�����".AsNoun(ESex.FEMALE, false);
				case EItemCategory.WEAPON:
					return "������".AsNoun(ESex.IT, false);
				case EItemCategory.MISSILE_WEAPON:
					return "������".AsNoun(ESex.IT, false) + "�����������".AsAdj();
				case EItemCategory.RINGS:
					return "������".AsNoun(ESex.IT, false);
				case EItemCategory.FOOD:
					return "����".AsNoun(ESex.FEMALE, false);
				case EItemCategory.POTION:
					return "�����".AsNoun(ESex.IT, false);
				case EItemCategory.MISSILES:
					return "������".AsNoun(ESex.MALE, false);
				case EItemCategory.TOOLS:
					return "����������".AsNoun(ESex.MALE, false);
				case EItemCategory.WANDS:
					return "����".AsNoun(ESex.MALE, false);
				case EItemCategory.BOOKS:
					return "�����".AsNoun(ESex.FEMALE, false);
				case EItemCategory.SCROLLS:
					return "c�����".AsNoun(ESex.MALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(ETerrains _e)
		{
			switch (_e)
			{
				case ETerrains.NONE:
					return "�����".AsNoun(ESex.IT, false);
				case ETerrains.GROUND:
					return "�����".AsNoun(ESex.FEMALE, false);
				case ETerrains.FRESH_WATER:
					return "����".AsNoun(ESex.FEMALE, false);
				case ETerrains.GRASS:
					return "�����".AsNoun(ESex.FEMALE, false);
				case ETerrains.SWAMP:
					return "������".AsNoun(ESex.IT, false);
				case ETerrains.LAVA:
					return "����".AsNoun(ESex.FEMALE, false);
				case ETerrains.ROAD:
					return "������".AsNoun(ESex.FEMALE, false);
				case ETerrains.RED_BRICK_WALL:
					return "�����".AsNoun(ESex.FEMALE, false) + "�� �������� �������".AsIm();
				case ETerrains.YELLOW_BRICK_WALL:
					return "�����".AsNoun(ESex.FEMALE, false) + "�� ������� �������".AsIm();
				case ETerrains.GRAY_BRICK_WALL:
					return "�����".AsNoun(ESex.FEMALE, false) + "�� ������ �������".AsIm();
				case ETerrains.STATUE:
					return "������".AsNoun(ESex.FEMALE, false);
				case ETerrains.WOOD_FLOOR_OAK:
					return "���".AsNoun(ESex.MALE, false) + "�������".AsAdj();
				case ETerrains.WOOD_FLOOR_MAPPLE:
					return "���".AsNoun(ESex.MALE, false) + "��������".AsAdj();
				case ETerrains.STONE_FLOOR:
					return "���".AsNoun(ESex.MALE, false) + "��������".AsAdj();
				case ETerrains.STONE_WALL:
					return "�����".AsNoun(ESex.FEMALE, false) + "��������".AsAdj();
				case ETerrains.FOREST:
					return "���".AsNoun(ESex.MALE, false);
				case ETerrains.SEA:
					return "����".AsNoun(ESex.IT, false);
				case ETerrains.DEEP_SEA:
					return "����".AsNoun(ESex.IT, false) + "��������".AsAdj();
				case ETerrains.DEEP_FRESH_WATER:
					return "����".AsNoun(ESex.FEMALE, false) + "��������".AsAdj();
				case ETerrains.COAST:
					return "�����".AsNoun(ESex.MALE, false);
				case ETerrains.LAKE_COAST:
					return "�����".AsNoun(ESex.MALE, false) + "������".AsAdj();
				case ETerrains.MOUNT:
					return "�����".AsNoun(ESex.PLURAL_FEMALE, false);
				case ETerrains.ETERNAL_SNOW:
					return "������".AsNoun(ESex.MALE, false);
				case ETerrains.SHRUBS:
					return "���������".AsNoun(ESex.MALE, false);
				case ETerrains.UP:
					return "����".AsNoun(ESex.MALE, false);
				case ETerrains.DOWN:
					return "���".AsNoun(ESex.MALE, false);
				case ETerrains.LEFT:
					return "����".AsNoun(ESex.IT, false);
				case ETerrains.RIGHT:
					return "�����".AsNoun(ESex.IT, false);
				case ETerrains.RED_BRICK_WINDOW:
					return "����".AsNoun(ESex.IT, false);
				case ETerrains.GRAY_BRICK_WINDOW:
					return "����".AsNoun(ESex.IT, false);
				case ETerrains.YELLOW_BRICK_WINDOW:
					return "����".AsNoun(ESex.IT, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(EActionCategory _e)
		{
			switch (_e)
			{
				case EActionCategory.MOVEMENT:
					return "�����������".AsNoun(ESex.PLURAL_FEMALE, false);
				case EActionCategory.COMBAT:
					return "���".AsNoun(ESex.MALE, false);
				case EActionCategory.ITEMS:
					return "��������������".AsNoun(ESex.IT, false) + "� ����������".AsIm();
				case EActionCategory.WORLD_INTERACTIONS:
					return "��������������".AsNoun(ESex.IT, false) + "� �����".AsIm();
				case EActionCategory.INFORMATION:
					return "����������".AsNoun(ESex.FEMALE, false);
				case EActionCategory.SYSTEM:
					return "�������".AsNoun(ESex.FEMALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(ETactics _e)
		{
			switch (_e)
			{
				case ETactics.PEACEFULL:
					return "��������".AsNoun(ESex.MALE, false);
				case ETactics.NORMAL:
					return "������".AsNoun(ESex.MALE, false);
				case ETactics.BERSERK:
					return "���������".AsNoun(ESex.IT, false);
				case ETactics.COWARD:
					return "������".AsNoun(ESex.FEMALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}
	}
}