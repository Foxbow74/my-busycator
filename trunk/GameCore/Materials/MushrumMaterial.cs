namespace GameCore.Materials
{
	abstract class MushrumMaterial : Material
	{
		protected MushrumMaterial(string _name) : base(string.Empty) { MushrumName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterial MaterialType { get { return EMaterial.MUSHRUM; } }

		public abstract int MushrumTileIndex { get; }

		public string MushrumName { get; private set; }
	}

	class MushrumMaterial0 : MushrumMaterial
	{
		public MushrumMaterial0() : base("гриб0") { }

		public override int MushrumTileIndex
		{
			get { return 0; }
		}
	}

	class MushrumMaterial1 : MushrumMaterial
	{
		public MushrumMaterial1() : base("гриб1") { }

		public override int MushrumTileIndex
		{
			get { return 1; }
		}
	}

	class MushrumMaterial2 : MushrumMaterial
	{
		public MushrumMaterial2() : base("гриб2") { }

		public override int MushrumTileIndex
		{
			get { return 2; }
		}
	}

	class MushrumMaterial3 : MushrumMaterial
	{
		public MushrumMaterial3() : base("гриб3") { }

		public override int MushrumTileIndex
		{
			get { return 3; }
		}
	}

	class MushrumMaterial4 : MushrumMaterial
	{
		public MushrumMaterial4() : base("гриб4") { }

		public override int MushrumTileIndex
		{
			get { return 4; }
		}
	}

	class MushrumMaterial5 : MushrumMaterial
	{
		public MushrumMaterial5() : base("гриб5") { }

		public override int MushrumTileIndex
		{
			get { return 5; }
		}
	}
}