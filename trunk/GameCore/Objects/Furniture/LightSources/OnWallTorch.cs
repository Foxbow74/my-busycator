using System;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Objects.Furniture.LightSources
{
	internal class OnWallTorch : LightSourceFurniture, ISpecial
	{
		private readonly EDirections m_direction;

		public OnWallTorch(LightSource _lightSource, EDirections _direction, Material _material)
			: base(_lightSource, _material) { m_direction = _direction; }

		public override ETileset Tileset { get { return ETileset.ON_WALL_LIGHT_SOURCE; } }

		public override string Name { get { return "факел"; } }

		public override EDirections Direction { get { return m_direction; } }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }
	}
}