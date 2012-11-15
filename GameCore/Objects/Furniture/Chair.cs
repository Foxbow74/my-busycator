using GameCore.Creatures;
using RusLanguage;

namespace GameCore.Objects.Furniture
{
	internal class Grave : FurnitureThing
	{
		public Grave(Material _material) : base(_material) 
		{
			Sex = ESex.FEMALE;
		}

		public override string Name { get { return "могила"; } }

		public override EThingCategory Category { get { return EThingCategory.LANDSCAPE; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.MINERAL; } }

		public override ETileset Tileset { get { return ETileset.GRAVE; } }

		public override void Resolve(Creature _creature) {  }
	}

	internal class Chair : FurnitureThing
	{
		public Chair(Material _material)
			: base(_material) { }

		public override ETileset Tileset { get { return ETileset.CHAIR; } }

		public override string Name { get { return "стул"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class Cabinet : FurnitureThing
	{
		public Cabinet(Material _material)
			: base(_material) { }

		public override ETileset Tileset { get { return ETileset.CABINET; } }

		public override string Name { get { return "шкаф"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class ArmorRack : FurnitureThing
	{
		public ArmorRack(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

		public override ETileset Tileset { get { return ETileset.ARMOR_RACK; } }

		public override string Name { get { return "стойка для брони"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class WeaponRack : FurnitureThing
	{
		public WeaponRack(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

		public override ETileset Tileset { get { return ETileset.WEAPON_RACK; } }

		public override string Name { get { return "стойка для оружия"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class Barrel : FurnitureThing
	{
		public Barrel(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

		public override ETileset Tileset { get { return ETileset.BARREL; } }

		public override string Name { get { return "бочка"; } }

		public override void Resolve(Creature _creature) { }
	}
}