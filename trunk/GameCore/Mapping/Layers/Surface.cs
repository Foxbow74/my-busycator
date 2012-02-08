namespace GameCore.Mapping.Layers
{
	public class Surface:WorldLayer
	{
		private EMapBlockTypes[,] m_worldMap;

		public EMapBlockTypes[,] WorldMap
		{
			get
			{
				if(m_worldMap==null)
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

		internal override System.Collections.Generic.IEnumerable<ETerrains> DefaultEmptyTerrains
		{
			get
			{
				yield return ETerrains.GRASS;
				yield return ETerrains.GROUND;
			}
		}
	}
}
