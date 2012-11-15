using RusLanguage;

namespace GameCore.Materials
{
	internal abstract class WoodMaterial : Material
	{
		protected WoodMaterial(string _name)
			: base(_name) { }

		public override EMaterial MaterialType { get { return EMaterial.WOOD; } }

		public abstract int TreeTileIndex { get; }
	}

	internal class OakMaterial : WoodMaterial
	{
		public OakMaterial()
			: base("дуб") { }

		public override FColor LerpColor { get { return FColor.DarkOak; } }

		public override int TreeTileIndex
		{
			get { return 0; }
		}
	}

	internal class AshMaterial : WoodMaterial
	{
		public AshMaterial()
			: base("ясень") { }

		public override FColor LerpColor { get { return FColor.Ash; } }
		public override int TreeTileIndex
		{
			get { return 1; }
		}
	}

	internal class MapleMaterial : WoodMaterial
	{
		public MapleMaterial()
			: base("клен") { }

		public override FColor LerpColor { get { return FColor.Maple; } }
		public override int TreeTileIndex
		{
			get { return 2; }
		}
	}

	internal class WalnutMaterial : WoodMaterial
	{
		public WalnutMaterial()
			: base("орех") { }

		public override FColor LerpColor { get { return FColor.Walnut; } }
		public override int TreeTileIndex
		{
			get { return 3; }
		}
	}

	internal class WillowMaterial : WoodMaterial
	{
		public WillowMaterial()
			: base("ива") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.WildWillow; } }
		public override int TreeTileIndex
		{
			get { return 4; }
		}
	}

	internal class PineMaterial : WoodMaterial
	{
		public PineMaterial()
			: base("сосна") { Sex = ESex.FEMALE; }

		public override FColor LerpColor { get { return FColor.PineTree; } }
		public override int TreeTileIndex
		{
			get { return 5; }
		}
	}
	
	internal class SpruceMaterial : WoodMaterial
	{
		public SpruceMaterial()
			: base("ель") { Sex = ESex.IT; }

		public override FColor LerpColor { get { return FColor.LimedSpruce; } }
		public override int TreeTileIndex
		{
			get { return 6; }
		}
	}
}