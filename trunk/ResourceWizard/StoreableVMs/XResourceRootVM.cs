using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	internal class XResourceRootVM : XObjectVM
	{
		#region Fields

		[X((int) EStoreKind.MONSTER_INFO)] private ICollection<XMonsterInfoVM> m_monsterInfos;
		[X((int) EStoreKind.NICKS_INFO)] private ICollection<XNicksInfoVM> m_nicksInfos;
		[X((int) EStoreKind.TERRAIN_SET)] private ICollection<XTerrainSetVM> m_terrainSets;
		[X((int) EStoreKind.THING_INFO)] private ICollection<XThingInfoVM> m_thingInfos;
		[X((int) EStoreKind.TILE_SET)] private ICollection<XTileSetVM> m_tileSets;

		#endregion

		#region Methods

		public void BeforeSave()
		{
			foreach (var vm in ThingInfos)
			{
				vm.BeforeSave();
			}
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
			ThingInfosObsCol = CreateObservableCollection(m_thingInfos);
		}

		#endregion

		#region Properties

		public override EStoreKind Kind
		{
			get { return EStoreKind.ALL; }
		}

		public ICollection<XThingInfoVM> ThingInfos
		{
			get { return m_thingInfos; }
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

		public ReadOnlyObservableCollection<XThingInfoVM> ThingInfosObsCol { get; private set; }

		public ICollection<XTileSetVM> TileSets
		{
			get { return m_tileSets; }
		}

		public ReadOnlyObservableCollection<XTileSetVM> TileSetsObsCol { get; private set; }

		#endregion
	}
}