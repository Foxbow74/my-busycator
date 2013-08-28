using System.Collections.Generic;
using System.Linq;

namespace GameUi
{
	public class TileSet
	{
		public TileSet(params ATile[] _tiles) { Tiles = _tiles.ToList(); }
		public TileSet() { Tiles = new List<ATile>(); }
		public ATile this[int _index] { get { return Tiles[_index%Tiles.Count]; } }
		public List<ATile> Tiles { get; private set; }
		public void AddTile(ATile _tile) { Tiles.Add(_tile); }
	}
}