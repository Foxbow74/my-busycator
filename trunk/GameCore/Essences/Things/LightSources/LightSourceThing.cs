﻿using GameCore.Mapping;
using GameCore.Misc;
using UnsafeUtils;

namespace GameCore.Essences.Things.LightSources
{
	internal abstract class LightSourceThing : Thing, ILightSource
	{
		private readonly LightSource m_lightSource;

        public override ETileset Tileset
        {
            get
            {
                return ETileset.LIGHT_SOURCES;
            }
        }

		protected LightSourceThing(LightSource _lightSource, Material _material) : base(_material) { m_lightSource = _lightSource; }

		#region ILightSource Members

		public void LightCells(LiveMap _liveMap, Point _point) { m_lightSource.LightCells(_liveMap, _point); }

		public int Radius { get { return m_lightSource.Radius; } }

		#endregion
	}
}