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
					return "Голова".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.NECK:
					return "Шея".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.BODY:
					return "Тело".AsNoun(ESex.IT, false);
				case EEquipmentPlaces.GIRGLE:
					return "Пояс".AsNoun(ESex.MALE, false);
				case EEquipmentPlaces.CLOACK:
					return "Накидка".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.RIGHT_HAND:
					return "рука".AsNoun(ESex.FEMALE, false) + "Правый".AsAdj();
				case EEquipmentPlaces.LEFT_HAND:
					return "рука".AsNoun(ESex.FEMALE, false) + "Левый".AsAdj();
				case EEquipmentPlaces.RIGHT_RING:
					return "Кольцо".AsNoun(ESex.IT, false) + "на правой руке".AsIm();
				case EEquipmentPlaces.LEFT_RING:
					return "Кольцо".AsNoun(ESex.IT, false) + "на левой руке".AsIm();
				case EEquipmentPlaces.BRACERS:
					return "Наручи".AsNoun(ESex.PLURAL, false);
				case EEquipmentPlaces.GAUNTLETS:
					return "Перчатки".AsNoun(ESex.PLURAL, false);
				case EEquipmentPlaces.BOOTS:
					return "Обувь".AsNoun(ESex.FEMALE, false);
				case EEquipmentPlaces.MISSILE_WEAPON:
					return "оружие".AsNoun(ESex.IT, false) + "Метательный".AsAdj();
				case EEquipmentPlaces.MISSILES:
					return "Снаряды".AsNoun(ESex.PLURAL, false);
				case EEquipmentPlaces.TOOL:
					return "Инструмент".AsNoun(ESex.MALE, false);
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
					return "шлем".AsNoun(ESex.MALE, false);
				case EItemCategory.NECKLACES:
					return "ожерелье".AsNoun(ESex.IT, false);
				case EItemCategory.WEAR:
					return "одежда".AsNoun(ESex.FEMALE, false);
				case EItemCategory.ARMOR:
					return "броня".AsNoun(ESex.FEMALE, false);
				case EItemCategory.GIRGLE:
					return "пояс".AsNoun(ESex.MALE, false);
				case EItemCategory.CLOACK:
					return "плащ".AsNoun(ESex.MALE, false);
				case EItemCategory.BRACERS:
					return "наручи".AsNoun(ESex.PLURAL, false);
				case EItemCategory.GAUNTLETS:
					return "перчатки".AsNoun(ESex.PLURAL_FEMALE, false);
				case EItemCategory.BOOTS:
					return "обувь".AsNoun(ESex.FEMALE, false);
				case EItemCategory.WEAPON:
					return "оружие".AsNoun(ESex.IT, false);
				case EItemCategory.MISSILE_WEAPON:
					return "оружие".AsNoun(ESex.IT, false) + "метательный".AsAdj();
				case EItemCategory.RINGS:
					return "кольцо".AsNoun(ESex.IT, false);
				case EItemCategory.FOOD:
					return "пища".AsNoun(ESex.FEMALE, false);
				case EItemCategory.POTION:
					return "зелье".AsNoun(ESex.IT, false);
				case EItemCategory.MISSILES:
					return "снаряд".AsNoun(ESex.MALE, false);
				case EItemCategory.TOOLS:
					return "инструмент".AsNoun(ESex.MALE, false);
				case EItemCategory.WANDS:
					return "жезл".AsNoun(ESex.MALE, false);
				case EItemCategory.BOOKS:
					return "книга".AsNoun(ESex.FEMALE, false);
				case EItemCategory.SCROLLS:
					return "cвиток".AsNoun(ESex.MALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(ETerrains _e)
		{
			switch (_e)
			{
				case ETerrains.NONE:
					return "ничто".AsNoun(ESex.IT, false);
				case ETerrains.GROUND:
					return "почва".AsNoun(ESex.FEMALE, false);
				case ETerrains.FRESH_WATER:
					return "вода".AsNoun(ESex.FEMALE, false);
				case ETerrains.GRASS:
					return "трава".AsNoun(ESex.FEMALE, false);
				case ETerrains.SWAMP:
					return "болото".AsNoun(ESex.IT, false);
				case ETerrains.LAVA:
					return "лава".AsNoun(ESex.FEMALE, false);
				case ETerrains.ROAD:
					return "дорога".AsNoun(ESex.FEMALE, false);
				case ETerrains.RED_BRICK_WALL:
					return "стена".AsNoun(ESex.FEMALE, false) + "из красного кирпича".AsIm();
				case ETerrains.YELLOW_BRICK_WALL:
					return "стена".AsNoun(ESex.FEMALE, false) + "из желтого кирпича".AsIm();
				case ETerrains.GRAY_BRICK_WALL:
					return "стена".AsNoun(ESex.FEMALE, false) + "из серого кирпича".AsIm();
				case ETerrains.STATUE:
					return "статуя".AsNoun(ESex.FEMALE, false);
				case ETerrains.WOOD_FLOOR_OAK:
					return "пол".AsNoun(ESex.MALE, false) + "дубовый".AsAdj();
				case ETerrains.WOOD_FLOOR_MAPPLE:
					return "пол".AsNoun(ESex.MALE, false) + "кленовый".AsAdj();
				case ETerrains.STONE_FLOOR:
					return "пол".AsNoun(ESex.MALE, false) + "каменный".AsAdj();
				case ETerrains.STONE_WALL:
					return "стена".AsNoun(ESex.FEMALE, false) + "каменный".AsAdj();
				case ETerrains.FOREST:
					return "лес".AsNoun(ESex.MALE, false);
				case ETerrains.SEA:
					return "море".AsNoun(ESex.IT, false);
				case ETerrains.DEEP_SEA:
					return "море".AsNoun(ESex.IT, false) + "глубокий".AsAdj();
				case ETerrains.DEEP_FRESH_WATER:
					return "вода".AsNoun(ESex.FEMALE, false) + "глубокий".AsAdj();
				case ETerrains.COAST:
					return "песок".AsNoun(ESex.MALE, false);
				case ETerrains.LAKE_COAST:
					return "песок".AsNoun(ESex.MALE, false) + "речной".AsAdj();
				case ETerrains.MOUNT:
					return "скалы".AsNoun(ESex.PLURAL_FEMALE, false);
				case ETerrains.ETERNAL_SNOW:
					return "ледник".AsNoun(ESex.MALE, false);
				case ETerrains.SHRUBS:
					return "кустарник".AsNoun(ESex.MALE, false);
				case ETerrains.UP:
					return "верх".AsNoun(ESex.MALE, false);
				case ETerrains.DOWN:
					return "низ".AsNoun(ESex.MALE, false);
				case ETerrains.LEFT:
					return "лево".AsNoun(ESex.IT, false);
				case ETerrains.RIGHT:
					return "право".AsNoun(ESex.IT, false);
				case ETerrains.RED_BRICK_WINDOW:
					return "окно".AsNoun(ESex.IT, false);
				case ETerrains.GRAY_BRICK_WINDOW:
					return "окно".AsNoun(ESex.IT, false);
				case ETerrains.YELLOW_BRICK_WINDOW:
					return "окно".AsNoun(ESex.IT, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(EActionCategory _e)
		{
			switch (_e)
			{
				case EActionCategory.MOVEMENT:
					return "Перемещения".AsNoun(ESex.PLURAL_FEMALE, false);
				case EActionCategory.COMBAT:
					return "Бой".AsNoun(ESex.MALE, false);
				case EActionCategory.ITEMS:
					return "Взаимодействие".AsNoun(ESex.IT, false) + "с предметами".AsIm();
				case EActionCategory.WORLD_INTERACTIONS:
					return "Взаимодействие".AsNoun(ESex.IT, false) + "с миром".AsIm();
				case EActionCategory.INFORMATION:
					return "Информация".AsNoun(ESex.FEMALE, false);
				case EActionCategory.SYSTEM:
					return "Система".AsNoun(ESex.FEMALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}

		public Noun AsNoun(ETactics _e)
		{
			switch (_e)
			{
				case ETactics.PEACEFULL:
					return "пацифизм".AsNoun(ESex.MALE, false);
				case ETactics.NORMAL:
					return "баланс".AsNoun(ESex.MALE, false);
				case ETactics.BERSERK:
					return "нападение".AsNoun(ESex.IT, false);
				case ETactics.COWARD:
					return "защита".AsNoun(ESex.FEMALE, false);
				default:
					throw new ArgumentOutOfRangeException("_e");
			}
		}
	}
}