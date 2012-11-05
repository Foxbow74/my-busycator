using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTerrainSetVM : XObjectVM
	{
		public XTerrainSetVM() { }

		[X("TERRAIN")]
		private readonly IXValue<int> m_eTerrains;

		[X("LIST")]
		private ICollection<XTerrainInfoVM> m_children;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET; }
		}

		public ETile Tile { get { return (ETile)m_eTerrains.Value; } set { m_eTerrains.Value = (int)value; } }

		public ICollection<XTerrainInfoVM> Children
		{
			get { return m_children; }
		}

		public ReadOnlyObservableCollection<XTerrainInfoVM> ChildrenObsCol { get; private set; }

		protected override void InstantiationFinished()
		{
			BindProperty(m_eTerrains, ()=>Tile);
			ChildrenObsCol = CreateObservableCollection(m_children);
		}
	}
}