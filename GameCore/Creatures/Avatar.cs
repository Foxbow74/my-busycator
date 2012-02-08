using System;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Ammo;
using GameCore.Objects.Weapons;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar() : base(Point.Zero, 100, EIntellectGrades.INT)
		{
			m_silence = false;
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