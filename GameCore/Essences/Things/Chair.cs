using GameCore.Creatures;

namespace GameCore.Essences.Things
{
	internal class Grave : Thing
	{
		public Grave(Material _material) : base(_material) 
		{
			Sex = ESex.FEMALE;
		}

		public override string Name { get { return "могила"; } }

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.MINERAL; } }

        public override int TileIndex { get { return 6; } }
	}

	internal class Chair : Thing
	{
		public Chair(Material _material)
			: base(_material) { }

        public override int TileIndex { get { return 2; } }

		public override string Name { get { return "стул"; } }
	}

	internal class Cabinet : Thing
	{
		public Cabinet(Material _material)
			: base(_material) { }

        public override int TileIndex { get { return 1; } }

		public override string Name { get { return "шкаф"; } }
	}

	internal class ArmorRack : Thing
	{
		public ArmorRack(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 3; } }

		public override string Name { get { return "стойка для брони"; } }
	}

	internal class WeaponRack : Thing
	{
		public WeaponRack(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 4; } }

		public override string Name { get { return "стойка для оружия"; } }
	}

	internal class Barrel : Thing
	{
		public Barrel(Material _material)
			: base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 5; } }

		public override string Name { get { return "бочка"; } }
	}
}