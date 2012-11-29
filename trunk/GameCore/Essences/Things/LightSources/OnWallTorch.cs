using System;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Essences.Things.LightSources
{
	internal class OnWallTorch : LightSourceThing, ISpecial
	{
		private readonly EDirections m_direction;

		public OnWallTorch(LightSource _lightSource, EDirections _direction, Material _material)
			: base(_lightSource, _material) { m_direction = _direction; }

        public override int TileIndex
        {
            get
            {
                return 1;
            }
        }

		public override string Name { get { return "факел"; } }

		public override EDirections Direction { get { return m_direction; } }
	}
}