using System;

namespace GameCore.Materials
{
	abstract class MushrumMaterial : Material
	{
		protected MushrumMaterial(string _name) : base(string.Empty) { MushrumName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterial MaterialType { get { return EMaterial.MUSHRUM; } }

		public abstract ETiles MushrumTile { get; }

		public string MushrumName { get; private set; }
	}

	class MushrumMaterial1 : MushrumMaterial
	{
		public MushrumMaterial1() : base("гриб1") { }

		public override ETiles MushrumTile { get { return ETiles.FOREST_MUSHROOM1; } }
	}

	class MushrumMaterial2 : MushrumMaterial
	{
		public MushrumMaterial2() : base("гриб2") { }

		public override ETiles MushrumTile { get { return ETiles.FOREST_MUSHROOM2; } }
	}

	class MushrumMaterial3 : MushrumMaterial
	{
		public MushrumMaterial3() : base("гриб3") { }

		public override ETiles MushrumTile { get { return ETiles.FOREST_MUSHROOM3; } }
	}

	class MushrumMaterial4 : MushrumMaterial
	{
		public MushrumMaterial4() : base("гриб4") { }

		public override ETiles MushrumTile { get { return ETiles.FOREST_MUSHROOM4; } }
	}

	class MushrumMaterial5 : MushrumMaterial
	{
		public MushrumMaterial5() : base("гриб5") { }

		public override ETiles MushrumTile { get { return ETiles.FOREST_MUSHROOM5; } }
	}
}