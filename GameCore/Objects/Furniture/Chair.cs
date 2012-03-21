using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	class Chair : FurnitureThing
	{
		public Chair(Material _material)
			: base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.CHAIR; }
		}

		public override string Name
		{
			get { return "стул"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
	class Cabinet : FurnitureThing
	{
		public Cabinet(Material _material)
			: base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.CABINET; }
		}

		public override string Name
		{
			get { return "шкаф"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
	class ArmorRack : FurnitureThing
	{
		public ArmorRack(Material _material)
			: base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.ARMOR_RACK; }
		}

		public override string Name
		{
			get { return "стойка для брони"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
	class WeaponRack : FurnitureThing
	{
		public WeaponRack(Material _material)
			: base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.WEAPON_RACK; }
		}

		public override string Name
		{
			get { return "стойка для оружия"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
	class Barrel : FurnitureThing
	{
		public Barrel(Material _material)
			: base(_material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.BARREL; }
		}

		public override string Name
		{
			get { return "бочка"; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}