#region

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

#endregion

namespace RGL1
{
	public enum TextureSet
	{
		REDJACK,
		RR_BRICK_01,
		RR_BRICK_02,
		RR_NATURAL_01,
		RR_NATURAL_02,
		GP_X16,
		NH,
	}

	public class Tile
	{
		public static int Size = 16;
		public readonly TextureSet Set;

		public Tile(TextureSet _set, int _x, int _y, Color _color)
		{
			Set = _set;
			Color = _color;
			Rectangle = new Rectangle(_x*Size, _y*Size, Size, Size);
		}

		public Tile(int _x, int _y, Color _color)
		{
			Set = TextureSet.REDJACK;
			Color = _color;
			Rectangle = new Rectangle(_x*Size, _y*Size, Size, Size);
		}

		protected Tile()
		{
		}

		public Color Color { get; private set; }

		public Rectangle Rectangle { get; private set; }
	}

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