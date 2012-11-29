using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameCore.Storage;
using GameCore.Storage.XResourceEssences;
using MagickSetting;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	internal class XResourceRootVM : XObjectVM, IXResourceEssenceProvider
	{
		#region Fields

#pragma warning disable 649
		[X((int) EStoreKind.MONSTER_INFO)] private ICollection<XMonsterInfoVM> m_monsterInfos;
		[X((int) EStoreKind.NICKS_INFO)] private ICollection<XNicksInfoVM> m_nicksInfos;
		[X((int) EStoreKind.TERRAIN_SET)] private ICollection<XTerrainSetVM> m_terrainSets;
        [X((int)EStoreKind.TILE_SET)] private ICollection<XTileSetVM> m_tileSets;
        [X((int)EStoreKind.COLOR)] private ICollection<XColorVM> m_colors;
		[X((int)EStoreKind.ESSENCE_INFO)] private ICollection<XResourceEssenceDummy> m_essenceProviders;
#pragma warning restore 649

		#endregion

		#region Methods

		public void BeforeSave()
		{
			foreach (var xTileSetVM in TileSets.Cast<XAbstractTileSetVM>().Union(TerrainSets))
			{
				foreach (var vm in xTileSetVM.Children)
				{
					vm.BeforeSave();
				}
			}
		}

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			TileSetsObsCol = CreateObservableCollection(m_tileSets);
			TerrainSetsObsCol = CreateObservableCollection(m_terrainSets);
		}

		#endregion

		#region Properties

		public override EStoreKind Kind
		{
			get { return EStoreKind.ALL; }
		}

		public ICollection<XMonsterInfoVM> MonsterInfos
		{
			get { return m_monsterInfos; }
		}

		public ICollection<XNicksInfoVM> NickInfos
		{
			get { return m_nicksInfos; }
		}

		public ICollection<XTerrainSetVM> TerrainSets
		{
			get { return m_terrainSets; }
		}

		public ReadOnlyObservableCollection<XTerrainSetVM> TerrainSetsObsCol { get; private set; }

		public ICollection<XTileSetVM> TileSets
		{
			get { return m_tileSets; }
		}

        public ICollection<XColorVM> Colors
        {
            get { return m_colors; }
        }

		public ReadOnlyObservableCollection<XTileSetVM> TileSetsObsCol { get; private set; }

		public ICollection<XResourceEssenceDummy> EssenceProviders
		{
			get { return m_essenceProviders; }
		}

		#endregion

		public TO CreateXResourceEssence<TO>(Guid _typeId) where TO : XObject
		{
			var helper = new XResourceEssenceDummy();
			EssenceProviders.Add(helper);
			helper.ProvierTypeId = _typeId;
			return Manager.Instance.XClient.Get<TO>(helper.Uid);
		}
	}
}