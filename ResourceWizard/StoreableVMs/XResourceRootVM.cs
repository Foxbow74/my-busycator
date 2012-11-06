using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XResourceRootVM : XObjectVM
	{
		[X((int)EStoreKind.MONSTER_INFO)]
		private ICollection<XMonsterInfoVM> m_monsterInfos;

		[X((int)EStoreKind.TILE_SET)]
		private ICollection<XTileSetVM> m_tileSets;

		[X((int)EStoreKind.TERRAIN_SET)]
		private ICollection<XTileSetVM> m_terrainSets;

		[X((int)EStoreKind.NICKS_INFO)]
		private ICollection<XNicksInfoVM> m_nicksInfos;

		public override EStoreKind Kind
		{
			get { return EStoreKind.ALL; }
		}

		public ICollection<XMonsterInfoVM> MonsterInfos
		{
			get { return m_monsterInfos; }
		}

		public ReadOnlyObservableCollection<XTileSetVM> TileSetsObsCol { get; private set; }

		public ICollection<XTileSetVM> TileSets
		{
			get { return m_tileSets; }
		}

		public ICollection<XNicksInfoVM> NickInfos { get { return m_nicksInfos; } }

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			TileSetsObsCol = CreateObservableCollection(m_tileSets);
		}
	}
}