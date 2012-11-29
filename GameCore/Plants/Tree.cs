using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Materials;

namespace GameCore.Plants
{
	class Tree : Thing
	{
		private readonly WoodMaterial m_material;

		public Tree(Material _material) : base(_material)
		{
			if(_material==null) return;

			m_material = (WoodMaterial)_material;
			Sex = m_material.Sex;
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

		public override string Name { get { return m_material.Name; } }

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.WOOD; } }

		public override void Resolve(Creature _creature) { }
	}
}
