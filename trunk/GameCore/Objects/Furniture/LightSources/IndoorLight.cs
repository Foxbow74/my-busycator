using System;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Objects.Furniture.LightSources
{
	internal class IndoorLight : LightSourceFurniture, ISpecial
	{
		public IndoorLight(LightSource _lightSource, Material _material)
			: base(_lightSource, _material) { }

		public override ETiles Tile { get { return ETiles.LIGHT_SOURCE; } }

		public override string Name { get { return "светильник"; } }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }
	}
}