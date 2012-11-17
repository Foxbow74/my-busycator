using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Materials;

namespace GameCore.Plants
{
    class Mushrum : Thing
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