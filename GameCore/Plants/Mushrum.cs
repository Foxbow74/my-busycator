using GameCore.AbstractLanguage;
using GameCore.Essences;
using GameCore.Materials;

namespace GameCore.Plants
{
    class Mushrum : Thing
    {
        private readonly MushrumMaterial m_material;

		public Mushrum(Material _material) : base(MushrumName(_material), _material) { m_material = (MushrumMaterial)_material; }

    	private static EALNouns MushrumName(Material _material)
    	{
			if (_material == null) return EALNouns.Mushrum;
    		return ((MushrumMaterial)_material).MushrumName;
    	}

    	public override ETileset Tileset { get { return ETileset.MUSHROOMS; } }

        public override int TileIndex
        {
            get
            {
                return m_material.MushrumTileIndex;
            }
        }

    	public override FColor LerpColor { get { return FColor.Empty; } }

        public override Noun Name { get { return m_material.MushrumName.AsNoun(); } }

        public override EMaterialType AllowedMaterialsType { get { return EMaterialType.MUSHRUM; } }
    }
}