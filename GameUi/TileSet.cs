using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameUi
{
	public class TileSet : ATile
	{
		private readonly ATile[] m_tiles;

		public TileSet(IEnumerable<ATile> _tiles)
		{
			m_tiles = _tiles.ToArray();
		}

		public ATile this[int _index]
		{
			get { return m_tiles[_index % m_tiles.Count()]; }
		}

		public override void Draw(int _x, int _y, Color _color)
		{
			throw new NotImplementedException();
		}
	}
}