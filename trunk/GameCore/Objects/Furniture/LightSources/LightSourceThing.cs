using System;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Objects.Furniture.LightSources
{
	abstract class LightSourceFurniture : FurnitureThing, ILightSource
	{
		private readonly LightSource m_lightSource;

		protected LightSourceFurniture(LightSource _lightSource, Material _material):base(_material)
		{
			m_lightSource = _lightSource;
		}

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			m_lightSource.LightCells(_liveMap, _point);
		}

		public int Radius
		{
			get { return m_lightSource.Radius; }
		}
	}
}
