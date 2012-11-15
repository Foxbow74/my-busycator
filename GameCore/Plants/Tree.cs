using GameCore.Creatures;
using GameCore.Materials;
using GameCore.Objects;

namespace GameCore.Plants
{
	class Tree : Furniture
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

		public override EThingCategory Category { get { return EThingCategory.LANDSCAPE; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.WOOD; } }

		public override void Resolve(Creature _creature) { }

		public override string GetFullName()
		{
			return Name;
		}
	}
	
	class Shrub: Furniture
	{
		private readonly ShrubMaterial m_material;

		public Shrub(Material _material) : base(_material) { m_material = (ShrubMaterial)_material; }

		public override ETileset Tileset { get { return ETileset.SHRUBS; } }

		public override int TileIndex
		{
			get
			{
				return m_material.ShroobTileIndex;
			}
		}

		public override void Resolve(Creature _creature) {  }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override string Name { get { return m_material.ShroobName; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.SHRUB; } }

		public override string GetFullName()
		{
			return Name;
		}
	}

	class Mushrum : Furniture
	{
		private readonly MushrumMaterial m_material;

		public Mushrum(Material _material) : base(_material) { m_material = (MushrumMaterial)_material; }

		public override ETileset Tileset { get { return ETileset.MUSHROOMS; } }

		public override int TileIndex
		{
			get
			{
				return m_material.MushrumTileIndex;
			}
		}

		public override void Resolve(Creature _creature) { }

		public override FColor LerpColor { get { return FColor.Empty; } }

		public override string Name { get { return m_material.MushrumName; } }

		public override EMaterial AllowedMaterials { get { return EMaterial.MUSHRUM; } }

		public override string GetFullName()
		{
			return Name;
		}
	}
}
