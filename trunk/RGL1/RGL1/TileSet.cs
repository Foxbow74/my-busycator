using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RGL1
{
	public class TileSet : Tile
	{
		private readonly Tile[] m_tiles;

		public TileSet(TextureSet _set, int _x, int _y, Color _color) : this(new[] {new Tile(_set, _x, _y, _color),})
		{
		}

		public TileSet(int _x, int _y, Color _color) : this(new[] {new Tile(_x, _y, _color),})
		{
		}

		public TileSet(IEnumerable<Tile> _tiles)
		{
			m_tiles = _tiles.ToArray();
		}

		public Tile this[int _index]
		{
			get { return m_tiles[_index%m_tiles.Count()]; }
		}
	}
}