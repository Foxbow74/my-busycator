using GameCore.Objects;
using Graphics;

namespace GameCore
{
	public class MapCell
	{
		public Point WorldCoords { get; private set; }

		public ETerrains Terrain { get; private set; }

		public EItems Item { get; private set; }

		public int BlockRandomSeed { get; private set; }

		internal MapCell(MapBlock _block, int _x, int _y, Point _worldCoords)
		{
			BlockRandomSeed = _block.RandomSeed;
			WorldCoords = _worldCoords;
			Terrain = _block.Map[_x, _y];
			Item = EItems.NONE;

			if (_block.ObjectsExists)
			{
				foreach (var tuple in _block.Objects)
				{
					if(tuple.Item2.X==_x && tuple.Item2.Y==_y)
					{
						var item = tuple.Item1;
						if(item is FakeItem)
						{
							Item = ((FakeItem)item).Item;
						}
					}
				}
			}

		}
	}
}