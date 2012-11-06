using System;
using GameCore.Storage;
using XTransport;
using XTransport.Client;

namespace ResourceWizard.StoreableVMs
{
	class ATileSetFactory: IXObjectFactory<EStoreKind>
	{
		public EStoreKind Kind
		{
			get { throw new System.NotImplementedException(); }
		}

		public IClientXObject<EStoreKind> CreateInstance(EStoreKind _kind)
		{
			switch (_kind)
			{
				case EStoreKind.TILE_SET:
					return new XTileSetVM();
				case EStoreKind.TERRAIN_SET:
					return new XTerrainSetVM();
				default:
					throw new ArgumentOutOfRangeException("_kind");
			}
		}
	}
}