using System;
using GameCore.Battle;
using GameCore.CreatureRoles;
using GameCore.Essences;
using GameCore.Essences.Ammo;
using GameCore.Essences.Tools;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;
using GameCore.Materials;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar(WorldLayer _surface)
			: base(_surface, 100, EIntellectGrades.INT)
		{
			Tactic = ETactics.NORMAL;
			AddRole(new AvatarRole());
			Equip(EEquipmentPlaces.RIGHT_HAND, EssenceHelper.GetFirstFoundedItem<Sword>());
			Equip(EEquipmentPlaces.MISSILE_WEAPON, EssenceHelper.GetFirstFoundedItem<CrossBow>());
			Equip(EEquipmentPlaces.MISSILES, new StackOfCrossBowBolts(EssenceHelper.GetMaterial<BrassMaterial>()) {Count = 100});
			Equip(EEquipmentPlaces.TOOL, EssenceHelper.GetFirstFoundedItem<Torch>());
		}

        public override ETileset Tileset { get { return ETileset.AVATAR; } }

        public override int TileIndex { get { return 2; } }

		public override string IntelligentName { get { return "Дима"; } }

		public override EFraction Fraction
		{
			get { return EFraction.AVATAR; }
		}

		public ETactics Tactic { get; set; }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }
		
		internal override CreatureBattleInfo CreateBattleInfo()
		{
			return new IntelligentBattleInfo(this);
		}
	}

	public enum ETactics
	{
		NORMAL,
		BERSERK,
		COWARD,
	}
}