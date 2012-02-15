﻿using System;
using System.Drawing;
using GameCore.Mapping.Layers;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Ammo;
using GameCore.Objects.Weapons;
using Point = GameCore.Misc.Point;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar(WorldLayer _surface)
			: base(_surface, Point.Zero, 100, EIntellectGrades.INT)
		{
			Light = new LightSource(this, 10, Color.Yellow.ToFColor());
			Silence = false;


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

		public LightSource Light { get; private set; }
	}
}