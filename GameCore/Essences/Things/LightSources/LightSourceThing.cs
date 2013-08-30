using GameCore.AbstractLanguage;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Essences.Things.LightSources
{
	public abstract class LightSourceThing : Thing, ILightSource
	{
		private readonly LightSource m_lightSource;

		protected LightSourceThing(EALNouns _name, LightSource _lightSource, Material _material) : base(_name, _material) { m_lightSource = _lightSource; }

		public override ETileset Tileset
		{
			get
			{
				return ETileset.LIGHT_SOURCES;
			}
		}

		public override FColor LerpColor
		{
			get
			{
				return m_lightSource.Color;
			}
		}

		#region ILightSource Members

		public void LightCells(LiveMap _liveMap, Point _point) { m_lightSource.LightCells(_liveMap, _point); }

		public int Radius { get { return m_lightSource.Radius; } }

		#endregion
	}
}