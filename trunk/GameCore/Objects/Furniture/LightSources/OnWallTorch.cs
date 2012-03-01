using System;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Objects.Furniture.LightSources
{
	class OnWallTorch:LightSourceFurniture, ISpecial
	{
		private readonly EDirections m_direction;

		public OnWallTorch(LightSource _lightSource, EDirections _direction) : base(_lightSource)
		{
			m_direction = _direction;
		}

		public override ETiles Tile
		{
			get { return ETiles.ON_WALL_LIGHT_SOURCE; }
		}

		public override string Name
		{
			get { return "факел"; }
		}

		public override EDirections Direction
		{
			get
			{
				return m_direction;
			}
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}
}