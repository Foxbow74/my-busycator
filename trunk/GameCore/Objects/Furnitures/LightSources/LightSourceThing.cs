using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Objects.Furnitures.LightSources
{
	internal abstract class LightSourceFurniture : Furniture, ILightSource
	{
		private readonly LightSource m_lightSource;

		protected LightSourceFurniture(LightSource _lightSource, Material _material) : base(_material) { m_lightSource = _lightSource; }

		#region ILightSource Members

		public void LightCells(LiveMap _liveMap, Point _point) { m_lightSource.LightCells(_liveMap, _point); }

		public int Radius { get { return m_lightSource.Radius; } }

		#endregion
	}
}