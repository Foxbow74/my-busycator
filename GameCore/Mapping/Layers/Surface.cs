using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Mapping.Layers
{
	public class Surface : WorldLayer
	{
		private EMapBlockTypes[,] m_worldMap;

		public EMapBlockTypes[,] WorldMap
		{
			get
			{
				if (m_worldMap == null)
				{
					var wmg = new WorldMapGenerator(WorldMapSize);
					m_worldMap = wmg.Generate();
				}
				return m_worldMap;
			}
		}

		public int WorldMapSize
		{
			get { return 150; }
		}

		internal override IEnumerable<ETerrains> DefaultEmptyTerrains
		{
			get
			{
				yield return ETerrains.GRASS;
				yield return ETerrains.GROUND;
			}
		}

		public override FColor Ambient
		{
			get { return new FColor(Color.FromArgb(255, 10, 10, 0)); }
		}

		public override FColor Lighted
		{
			get { return new FColor(Color.FromArgb(255, 255, 255, 100)); }
		}
	}
}