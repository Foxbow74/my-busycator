using System;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.PathFinding;

namespace GameCore.Objects.Furniture.LightSources
{
	class OnWallTorch:LightSourceFurniture, ISpecial
	{
		private readonly EDirections m_direction;

		public OnWallTorch(LightSource _lightSource, EDirections _direction, Material _material)
			: base(_lightSource, _material)
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

	class IndoorLight : LightSourceFurniture, ISpecial
	{
		public IndoorLight(LightSource _lightSource, Material _material)
			: base(_lightSource, _material)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.LIGHT_SOURCE; }
		}

		public override string Name
		{
			get { return "светильник"; }
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}
}