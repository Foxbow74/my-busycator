using GameCore.Creatures;
using RusLanguage;

namespace GameCore.Objects.Furnitures
{
	internal class Grave : Furniture
	{
		public Grave(Material _material) : base(_material) 
		{
			Sex = ESex.FEMALE;
		}

		public override string Name { get { return "могила"; } }

		public override EThingCategory Category { get { return EThingCategory.LANDSCAPE; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.MINERAL; } }

        public override int TileIndex { get { return 6; } }

		public override void Resolve(Creature _creature) {  }
	}

	internal class Chair : Furniture
	{
		public Chair(Material _material)
			: base(_material) { }

        public override int TileIndex { get { return 2; } }

		public override string Name { get { return "стул"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class Cabinet : Furniture
	{
		public Cabinet(Material _material)
			: base(_material) { }

        public override int TileIndex { get { return 1; } }

		public override string Name { get { return "шкаф"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class ArmorRack : Furniture
	{
		public ArmorRack(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 3; } }

		public override string Name { get { return "стойка для брони"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class WeaponRack : Furniture
	{
		public WeaponRack(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 4; } }

		public override string Name { get { return "стойка для оружия"; } }

		public override void Resolve(Creature _creature) { }
	}

	internal class Barrel : Furniture
	{
		public Barrel(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 5; } }

		public override string Name { get { return "бочка"; } }

		public override void Resolve(Creature _creature) { }
	}
}