using System;
using GameCore.CreatureRoles;
using GameCore.Mapping.Layers;
using GameCore.Materials;
using GameCore.Essences;
using GameCore.Essences.Ammo;
using GameCore.Essences.Tools;
using GameCore.Essences.Weapons;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar(WorldLayer _surface)
			: base(_surface, 100, EIntellectGrades.INT)
		{
			AddRole(new AvatarRole());
			Equip(EEquipmentPlaces.RIGHT_HAND, EssenceHelper.GetFirstFoundedItem<Sword>());
			Equip(EEquipmentPlaces.MISSILE_WEAPON, EssenceHelper.GetFirstFoundedItem<CrossBow>());
			Equip(EEquipmentPlaces.MISSILES, new StackOfCrossBowBolts(EssenceHelper.GetMaterial<BrassMaterial>()) {Count = 100});
			Equip(EEquipmentPlaces.TOOL, EssenceHelper.GetFirstFoundedItem<Torch>());
		}

        public override ETileset Tileset { get { return ETileset.AVATAR; } }

        public override int TileIndex { get { return 2; } }

		public override string IntelligentName { get { return "Дима"; } }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }
	}
}