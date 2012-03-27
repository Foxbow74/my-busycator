namespace GameCore.Materials
{
	abstract class ShrubMaterial: Material
	{
		protected ShrubMaterial(string _name) : base(string.Empty) { ShroobName = _name; }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override EMaterial MaterialType { get { return EMaterial.SHRUB; } }

		public abstract ETiles ShroobTile { get; }

		public string ShroobName { get; private set; }
	}

	class ShrubMaterial1 : ShrubMaterial
	{
		public ShrubMaterial1() : base("куст1") { }
		public override ETiles ShroobTile { get { return ETiles.FOREST_SHRUB_1; } }
	}

	class ShrubMaterial2 : ShrubMaterial
	{
		public ShrubMaterial2() : base("куст2") { }
		public override ETiles ShroobTile { get { return ETiles.FOREST_SHRUB_2; } }
	}

	class ShrubMaterial3 : ShrubMaterial
	{
		public ShrubMaterial3() : base("куст3") { }
		public override ETiles ShroobTile { get { return ETiles.FOREST_SHRUB_3; } }
	}

	class ShrubMaterial4 : ShrubMaterial
	{
		public ShrubMaterial4() : base("куст4") { }
		public override ETiles ShroobTile { get { return ETiles.FOREST_SHRUB_4; } }
	}

	class ShrubMaterial5 : ShrubMaterial
	{
		public ShrubMaterial5() : base("куст5") { }
		public override ETiles ShroobTile { get { return ETiles.FOREST_SHRUB_5; } }
	}
}
