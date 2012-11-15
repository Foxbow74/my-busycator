namespace GameCore.Materials
{
	abstract class ShrubMaterial: Material
	{
		protected ShrubMaterial(string _name) : base(string.Empty) { ShroobName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterial MaterialType { get { return EMaterial.SHRUB; } }

		public string ShroobName { get; private set; }
		
		public abstract int ShroobTileIndex { get; }
	}

	class ShrubMaterial0 : ShrubMaterial
	{
		public ShrubMaterial0() : base("куст0") { }

		public override int ShroobTileIndex
		{
			get { return 0; }
		}
	}

	class ShrubMaterial1 : ShrubMaterial
	{
		public ShrubMaterial1() : base("куст1") { }

		public override int ShroobTileIndex
		{
			get { return 1; }
		}
	}

	class ShrubMaterial2 : ShrubMaterial
	{
		public ShrubMaterial2() : base("куст2") { }
		public override int ShroobTileIndex
		{
			get { return 2; }
		}
	}

	class ShrubMaterial3 : ShrubMaterial
	{
		public ShrubMaterial3() : base("куст3") { }
		public override int ShroobTileIndex
		{
			get { return 3; }
		}

	}

	class ShrubMaterial4 : ShrubMaterial
	{
		public ShrubMaterial4() : base("куст4") { }
		public override int ShroobTileIndex
		{
			get { return 4; }
		}

	}

	class ShrubMaterial5 : ShrubMaterial
	{
		public ShrubMaterial5() : base("куст5") { }
		public override int ShroobTileIndex
		{
			get { return 5; }
		}

	}
}
