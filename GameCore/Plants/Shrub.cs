using GameCore.AbstractLanguage;
using GameCore.Essences;
using GameCore.Materials;

namespace GameCore.Plants
{
    class Shrub: Thing
    {
        private readonly ShrubMaterial m_material;

		public Shrub(Material _material) : base(ShroobName(_material), _material) { m_material = (ShrubMaterial)_material; }

    	private static Noun ShroobName(Material _material)
    	{
			if (_material == null) return null;
    		return ((ShrubMaterial)_material).ShroobName;
    	}

    	public override ETileset Tileset { get { return ETileset.SHRUBS; } }

        public override int TileIndex
        {
            get
            {
                return m_material.ShroobTileIndex;
            }
        }

    	public override FColor LerpColor { get { return FColor.Empty; } }

        public override EMaterialType AllowedMaterialsType { get { return EMaterialType.SHRUB; } }
    }
}