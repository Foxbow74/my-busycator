using System.Collections.Generic;
using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XResourceRoot : XObject
	{
		[X((int)EStoreKind.MONSTER_INFO)]private ICollection<XMonsterInfo> m_monsterInfos;

		[X((int)EStoreKind.TERRAIN_SET)]private ICollection<XTerrainSet> m_terrainSets;
		
		[X((int)EStoreKind.TILE_SET)]private ICollection<XTileSet> m_tileSets;

		[X((int)EStoreKind.NICKS_INFO)]private ICollection<XNicksInfo> m_nicksInfos;

		public override EStoreKind Kind
		{
			get { return EStoreKind.ALL; }
		}

		public ICollection<XMonsterInfo> MonsterInfos
		{
			get { return m_monsterInfos; }
		}

		public ICollection<XTerrainSet> TerrainSets
		{
			get { return m_terrainSets; }
		}

		public ICollection<XTileSet> TileSets
		{
			get { return m_tileSets; }
		}

		public ICollection<XNicksInfo> NickInfos { get { return m_nicksInfos; } }
	}
}
