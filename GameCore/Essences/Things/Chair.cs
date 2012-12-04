using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	internal class Grave : Thing
	{
		public Grave(Material _material) : base("могила".AsNoun(ESex.FEMALE, false), _material) 
		{
			Sex = ESex.FEMALE;
		}

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.MINERAL; } }

        public override int TileIndex { get { return 6; } }
	}

	internal class Chair : Thing
	{
		public Chair(Material _material): base("стул".AsNoun(ESex.MALE, false),_material) { }

        public override int TileIndex { get { return 2; } }
	}

	internal class Cabinet : Thing
	{
		public Cabinet(Material _material): base("шкаф".AsNoun(ESex.MALE, false), _material) { }

        public override int TileIndex { get { return 1; } }

		public override Noun Name { get { return "шкаф".AsNoun(ESex.MALE, false); } }
	}

	internal class ArmorRack : Thing
	{
		public ArmorRack(Material _material)
			: base("стойка".AsNoun(ESex.FEMALE, false) + " для брони".AsIm(), _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 3; } }
	}

	internal class WeaponRack : Thing
	{
		public WeaponRack(Material _material)
			: base("стойка".AsNoun(ESex.FEMALE, false) + " для оружия".AsIm(), _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 4; } }
	}

	internal class Barrel : Thing
	{
		public Barrel(Material _material)
			: base("бочка".AsNoun(ESex.FEMALE, false), _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 5; } }
	}
}