using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	internal class Grave : Thing
	{
		public Grave(Material _material) : base(EALNouns.Grave, _material) 
		{
			Sex = ESex.FEMALE;
		}

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.MINERAL; } }

        public override int TileIndex { get { return 6; } }
	}

	internal class Chair : Thing
	{
		public Chair(Material _material): base(EALNouns.Chair, _material) { }

        public override int TileIndex { get { return 2; } }
	}

	internal class Cabinet : Thing
	{
		public Cabinet(Material _material): base(EALNouns.Cabinet, _material) { }

        public override int TileIndex { get { return 1; } }
	}

	internal class ArmorRack : Thing
	{
		public ArmorRack(Material _material)
			: base(EALNouns.ArmorRack, _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 3; } }
	}

	internal class WeaponRack : Thing
	{
		public WeaponRack(Material _material)
			: base(EALNouns.WeaponRack, _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 4; } }
	}

	internal class Barrel : Thing
	{
		public Barrel(Material _material)
			: base(EALNouns.Barrel, _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 5; } }
	}
}