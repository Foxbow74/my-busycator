using GameCore.AbstractLanguage;
using GameCore.Misc;

namespace GameCore.Essences.Things.LightSources
{
	internal class OnWallTorch : LightSourceThing, ISpecial
	{
		private readonly EDirections m_direction;

		public OnWallTorch(LightSource _lightSource, EDirections _direction, Material _material)
			: base(EALNouns.Torch, _lightSource, _material) { m_direction = _direction; }

        public override int TileIndex
        {
            get
            {
                return 1;
            }
        }

		public override EDirections Direction { get { return m_direction; } }
	}
}