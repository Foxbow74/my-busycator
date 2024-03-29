﻿using System.Collections.Generic;
using GameCore.Storage;
using GameCore.Storage.XResourceEssences;
using XTransport;

namespace GameCore.Storeable
{
	internal class XResourceRoot : XObject
	{
#pragma warning disable 649
		[X((int) EStoreKind.MONSTER_INFO)] private ICollection<XMonsterInfo> m_monsterInfos;
		[X((int) EStoreKind.TERRAIN_SET)] private ICollection<XTerrainSet> m_terrainSets;
		[X((int) EStoreKind.TILE_SET)] private ICollection<XTileSet> m_tileSets;
		//[X((int) EStoreKind.NICKS_INFO)] private ICollection<XNicksInfo> m_nicksInfos;
		[X((int) EStoreKind.COLOR)] private ICollection<XColor> m_colors;
		[X((int)EStoreKind.ESSENCE_INFO)]private ICollection<XResourceEssenceDummy> m_essenceProviders;
#pragma warning restore 649

		public override EStoreKind Kind
		{
			get { return EStoreKind.ALL; }
		}

		public ICollection<XMonsterInfo> MonsterInfos
		{
			get { return m_monsterInfos; }
		}

		public ICollection<XResourceEssenceDummy> EssenceProviders
		{
			get { return m_essenceProviders; }
		}

		public ICollection<XTerrainSet> TerrainSets
		{
			get { return m_terrainSets; }
		}

		public ICollection<XTileSet> TileSets
		{
			get { return m_tileSets; }
		}

		public ICollection<XColor> Colors
		{
			get { return m_colors; }
		}

        //public ICollection<XNicksInfo> NickInfos
        //{
        //    get { return m_nicksInfos; }
        //}
	}
}
