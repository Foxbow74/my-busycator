using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Materials;

namespace GameCore.Plants
{
    class Shrub: Thing
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
}