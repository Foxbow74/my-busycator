using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Misc;

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
			get { return m_tiles[_index%m_tiles.Count()]; }
		}

		public override void Draw(int _x, int _y, FColor _color, FColor _background)
		{
			throw new NotImplementedException();
		}

		public override void Draw(Point _point, FColor _color, FColor _background)
		{
			throw new NotImplementedException();
		}

		public override void FogIt(int _col, int _row)
		{
			throw new NotImplementedException();
		}
	}
}