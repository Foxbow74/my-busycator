using System;
using GameCore.CreatureRoles;
using GameCore.Mapping.Layers;
using GameCore.Materials;
using GameCore.Objects;
using GameCore.Objects.Ammo;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar(WorldLayer _surface)
			: base(_surface, 100, EIntellectGrades.INT)
		{
			AddRole(new AvatarRole());
			//Equip(EEquipmentPlaces.MISSILE_WEAPON, ETileset.CROSSBOW.GetItem());
			Equip(EEquipmentPlaces.MISSILES, new StackOfCrossBowBolts(ThingHelper.GetMaterial<BrassMaterial>()) {Count = 100});
			//Equip(EEquipmentPlaces.TOOL, ETileset.TORCH.GetItem());
		}

        public override ETileset Tileset { get { return ETileset.AVATAR; } }

        public override int TileIndex { get { return 2; } }

		public override string IntelligentName { get { return "Дима"; } }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }
	}
}