using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Objects.Furniture.LightSources
{
	abstract class LightSourceFurniture : FurnitureThing, ILightSource
	{
		private readonly LightSource m_lightSource;

		protected LightSourceFurniture(LightSource _lightSource)
		{
			m_lightSource = _lightSource;
		}

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			m_lightSource.LightCells(_liveMap, _point);
		}
	}
}
