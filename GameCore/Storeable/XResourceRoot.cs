using System.Collections.Generic;
using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XResourceRoot : XObject
	{
		[X((int)EStoreKind.MONSTER_INFO)]private ICollection<XMonsterInfo> m_monsterInfos;
		
		[X((int)EStoreKind.TILE_INFO)]private ICollection<XTileInfo> m_tileInfos;
		
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

		public ICollection<XTileInfo> TileInfos
		{
			get { return m_tileInfos; }
		}

		public ICollection<XTileSet> TileSets
		{
			get { return m_tileSets; }
		}

		public ICollection<XNicksInfo> NickInfos { get { return m_nicksInfos; } }
	}
}
