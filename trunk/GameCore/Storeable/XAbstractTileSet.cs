using System.Collections.Generic;
using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	abstract class XAbstractTileSet : XObject
	{
#pragma warning disable 649
		[X("LIST")] private ICollection<XTileInfo> m_children;
#pragma warning restore 649

		public ICollection<XTileInfo> Children
		{
			get { return m_children; }
		}

	}
}