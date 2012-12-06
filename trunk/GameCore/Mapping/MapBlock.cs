using System;
using System.Collections.Generic;
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

		//public void AddCreature(Creature _creature, Point _inBlockCoords)
		//{
		//    if (_creature is FakedCreature)
		//    {
		//        _creature = (Creature)((FakedCreature)_creature).Essence.Clone(World.TheWorld.Avatar);
		//    }
		//    List<Creature> list;
		//    if(!Creatures.TryGetValue(_inBlockCoords, out list))
		//    {
		//        list = new List<Creature>();
		//        Creatures.Add(_inBlockCoords, list);
		//    }
		//    list.Add(_creature);
		//}

		//public void RemoveCreature(Creature _creature)
		//{
		//    foreach (var creature in Creatures)
		//    {
		//        if(creature.Value.Contains(_creature))
		//        {
		//            creature.Value.Remove(_creature);
		//            return;
		//        }
		//    }
		//    throw new ApplicationException();
		//}

		//public void AvatarLeftLayer()
		//{
		//    foreach (var pair in Creatures)
		//    {
		//        foreach (var creature in pair.Value)
		//        {
		//            creature.ClearActPool();
		//        }
		//    }
		//}
		public void CreaturesAdd(Creature _creature, Point _inBlockCoords)
		{
#if DEBUG
			if (Creatures.Any(_pair=>_pair.Value==_inBlockCoords))
			{
				throw new ApplicationException();
			}
#endif
			Creatures.Add(_creature, _inBlockCoords);
		}
	}
}