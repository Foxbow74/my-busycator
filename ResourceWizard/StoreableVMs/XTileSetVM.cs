using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XTileSetVM : XAbstractTileSetVM
	{
		[X("TILE")]
		private readonly IXValue<int> m_eTile;

		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_SET; }
		}
        
	    public IEnumerable<string> References
	    {
	        get
	        {
                return Manager.Instance.GetThings(Key)
                    .GroupBy(_thing => _thing.TileIndex)
                    .OrderBy(_things => _things.Key)
                    .Select(_grouping => _grouping.Key.ToString("00") + " - " + 
                        string.Join(" & ", _grouping.GroupBy(_thing => _thing.Name).Select(_things =>
                                                                                               {
                                                                                                   var materials = string.Join(", ",_things.Select(_t =>_t.Material.Name));
                                                                                                   return string.Format("{0} ({1})", _things.Key, materials);
                                                                                               })));
            }
	    }

		public ETileset Key { get { return (ETileset)m_eTile.Value; } set { m_eTile.Value = (int)value; } }

        public string KeyName { get { return Key.ToString(); } }
        
		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_eTile, () => Key);
		}
	}
}