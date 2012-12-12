using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public class MapBlock : BaseMapBlock
	{
		public MapBlock(Point _blockId) : base(_blockId)
		{
			Creatures = new Dictionary<Creature, Point>();
				//new List<CreaturePosition>();
			SeenCells = new uint[Constants.MAP_BLOCK_SIZE];
		}

		public MapBlock(Point _blockId, BaseMapBlock _baseMapBlock) : this(_blockId)
		{
			foreach (var point in Rect.AllPoints)
			{
				Map[point.X, point.Y] = _baseMapBlock.Map[point.X, point.Y];
			}
			foreach (var room in _baseMapBlock.Rooms)
			{
				Rooms.Add(room);
			}
			RandomSeed = _baseMapBlock.RandomSeed;
		}

		public Dictionary<Creature, Point> Creatures { get; private set; }

		//public List<CreaturePosition> Creatures { get; private set; }

		public UInt32[] SeenCells { get; private set; }

		public IEnumerable<Tuple<ILightSource, Point>> LightSources
		{
			get
			{
				foreach (var tuple in Objects)
				{
					if (tuple.Item1 is ILightSource)
					{
						yield return new Tuple<ILightSource, Point>((ILightSource)tuple.Item1, tuple.Item2);
					}
					else if (tuple.Item1.Light != null)
					{
						yield return new Tuple<ILightSource, Point>(tuple.Item1.Light, tuple.Item2);
					}
				}
			}
		}

		public void CreaturesAdd(Creature _creature, Point _inBlockCoords)
		{
			if(Creatures.Any(_pair => _pair.Value == _inBlockCoords))
			{
				throw new ApplicationException();
			}
			//var busy = new List<Point>();
			//while (Creatures.Any(_pair => _pair.Value == _inBlockCoords))
			//{
			//    busy.Add(_inBlockCoords);
			//    var lt = _creature[0, 0].InBlockCoords;

			//    foreach (var point in new Point(Constants.MAP_BLOCK_SIZE / 2, Constants.MAP_BLOCK_SIZE / 2).GetSpiral(Constants.MAP_BLOCK_SIZE / 2 - 1))
			//    {

			//        if (_creature[point - lt].GetIsPassableBy(_creature) > 0 && !busy.Contains(point))
			//        {
			//            _inBlockCoords = point;
			//            break;
			//        }
			//    }
			//}

			Creatures.Add(_creature, _inBlockCoords);
			//if (_creature.GeoInfo != null)
			//{
			//    _creature.GeoInfo.WorldCoords = WorldCoords + _inBlockCoords;
			//}
		}
	}
}