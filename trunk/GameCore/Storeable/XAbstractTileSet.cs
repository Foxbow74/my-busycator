using System.Collections.Generic;
using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	abstract class XAbstractTileSet : XObject
	{
		[X("LIST")]
		private ICollection<XTileInfo> m_children;

		public ICollection<XTileInfo> Children
		{
			get { return m_children; }
		}

	}
}