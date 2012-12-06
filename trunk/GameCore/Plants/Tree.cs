using GameCore.AbstractLanguage;
using GameCore.Essences;
using GameCore.Materials;

namespace GameCore.Plants
{
	public class Tree : Thing
	{
		private readonly WoodMaterial m_material;

		public Tree(Material _material) : base(TreeName(_material), _material)
		{
			if(_material==null) return;

			m_material = (WoodMaterial)_material;

			Sex = m_material.Sex;
		}

		private static EALNouns TreeName(Material _material)
		{
			if (_material == null) return EALNouns.Tree;
			return ((WoodMaterial)_material).TreeName;
		}

		public override ETileset Tileset { get { return ETileset.TREES; } }

		public override int TileIndex
		{
			get
			{
				return m_material.TreeTileIndex;
			}
		}

		public override FColor LerpColor
		{
			get { return FColor.Empty; }
		}

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.WOOD; } }
	}
}
