using RusLanguage;

namespace GameCore.Materials
{
	internal abstract class WoodMaterial : Material
	{
		protected WoodMaterial(string _name)
			: base(_name) { }

		public override EMaterial MaterialType { get { return EMaterial.WOOD; } }

		public abstract ETiles TreeTile { get; }
	}

	internal class OakMaterial : WoodMaterial
	{
		public OakMaterial()
			: base("дуб") { }

		public override FColor LerpColor { get { return FColor.DarkOak; } }

		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_OAK; } }
	}

	internal class AshMaterial : WoodMaterial
	{
		public AshMaterial()
			: base("ясень") { }

		public override FColor LerpColor { get { return FColor.Ash; } }
		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_ASH; } }
	}

	internal class MapleMaterial : WoodMaterial
	{
		public MapleMaterial()
			: base("клен") { }

		public override FColor LerpColor { get { return FColor.Maple; } }
		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_MAPLE; } }
	}

	internal class WalnutMaterial : WoodMaterial
	{
		public WalnutMaterial()
			: base("орех") { }

		public override FColor LerpColor { get { return FColor.Walnut; } }
		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_WALNUT; } }
	}

	internal class WillowMaterial : WoodMaterial
	{
		public WillowMaterial()
			: base("ива") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.WildWillow; } }
		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_WILLOW; } }
	}

	internal class PineMaterial : WoodMaterial
	{
		public PineMaterial()
			: base("сосна") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.PineTree; } }
		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_PINE; } }
	}
	
	internal class SpruceMaterial : WoodMaterial
	{
		public SpruceMaterial()
			: base("ель") { Sex = ESex.IT; }

		public override FColor LerpColor { get { return FColor.LimedSpruce; } }
		public override ETiles TreeTile { get { return ETiles.FOREST_TREE_SPRUCE; } }
	}
}