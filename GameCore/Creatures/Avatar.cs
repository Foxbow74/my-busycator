using System;
using GameCore.Mapping.Layers;
using GameCore.Objects;
using GameCore.Objects.Ammo;
using GameCore.Objects.Weapons;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar(WorldLayer _surface)
			: base(_surface, 100, EIntellectGrades.INT)
		{
			Equip(EEquipmentPlaces.MISSILE_WEAPON, new CrossBow());
			Equip(EEquipmentPlaces.MISSILES, new StackOfCrossBowBolts());
		}

		public override ETiles Tile
		{
			get { return ETiles.AVATAR; }
		}

		public override string Name
		{
			get { return "аватар"; }
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}
}