using System;
using System.Diagnostics;
using System.Linq;
using GameCore.Essences;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public class LiveMapBlock
	{
		private readonly LiveMap m_liveMap;
		private readonly int m_liveMapBlockIndex;

		public LiveMapBlock(LiveMap _liveMap, Point _liveMapBlockId, int _liveMapBlockIndex)
		{
			m_liveMap = _liveMap;
			LiveMapBlockId = _liveMapBlockId;
			m_liveMapBlockIndex = _liveMapBlockIndex;
			LiveCoords = LiveMapBlockId*Constants.MAP_BLOCK_SIZE;

			foreach (var point in LiveCoords.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y] = new LiveMapCell(this, point);
			}
		}

		public MapBlock MapBlock { get; private set; }

		/// <summary>
		/// Флаг, указывающий, что блок является границей живой карты. Существа, попадающие на него "выводятся из игры" и в активных действиях участия не принимают.
		/// </summary>
		public bool IsBorder { get; set; }

		//public IEnumerable<Creature> Creatures
		//{
		//    get
		//    {
		//        var arr = MapBlock.Creatures.SelectMany(e => e.Value).ToArray();
		//        return arr;
		//    }
		//}

		public Point LiveMapBlockId { get; private set; }

		public void ClearTemp()
		{
			foreach (var point in LiveCoords.GetAllBlockPoints())
			{ 
				m_liveMap.Cells[point.X, point.Y].ClearTemp();
			}
		}

		/// <summary>
		/// Мировые координаты левого верхнего угла блока
		/// </summary>
		public Point WorldCoords { get; private set; }

		public Point LiveCoords { get; private set; }

		private void Fill()
		{
			var rnd = new Random(MapBlock.RandomSeed);

			WorldCoords = MapBlock.BlockId*Constants.MAP_BLOCK_SIZE;

			foreach (var point in LiveCoords.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y].SetMapCell(MapBlock, point - LiveCoords, (float)rnd.NextDouble(), point, m_liveMap);
			}
			foreach (var tuple in MapBlock.Objects)
			{
				var cellId = tuple.Item2 + LiveCoords;
				if (tuple.Item1 is Item)
				{
					m_liveMap.Cells[cellId.X, cellId.Y].AddItemIntenal((Item) tuple.Item1);
				}
				else if (tuple.Item1.Is<Thing>())
				{
					m_liveMap.Cells[cellId.X, cellId.Y].Thing = (Thing) tuple.Item1;
				}
			}
			foreach (var pair in MapBlock.Creatures)
			{
				World.TheWorld.CreatureManager.AddCreature(pair.Key, WorldCoords + pair.Value, LiveCoords + pair.Value);
			}
			MapBlock.Creatures.Clear();
			Debug.WriteLine(MapBlock.BlockId + " CLEARED " + MapBlock.Creatures.Count);
		}

		public void UpdatePathFinderMapCoords()
		{
			var basePoint = (MapBlock.BlockId - World.TheWorld.AvatarBlockId + LiveMap.ActiveQpoint) * Constants.MAP_BLOCK_SIZE;
			foreach (var point in LiveCoords.GetAllBlockPoints())
			{
				var pnt = basePoint + BaseMapBlock.GetInBlockCoords(point);
				m_liveMap.Cells[point.X, point.Y].PathMapCoords = pnt;
			}
		}

		public void SetMapBlock(MapBlock _mapBlock)
		{
			MapBlock = _mapBlock;
			Fill();
		}

		public override string ToString() { return LiveMapBlockId + " MB:" + (MapBlock == null ? "<null>" : MapBlock.BlockId.ToString()); }

		public void Clear()
		{
			var rct = new Rct(WorldCoords, Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE);
			var arr = World.TheWorld.CreatureManager.InfoByCreature.Values.Where(_info => rct.Contains(_info.WorldCoords)).ToArray();
			foreach (var info in arr)
			{
				World.TheWorld.CreatureManager.ExcludeCreature(info.Creature);
			}

			if (MapBlock == null) return;
			MapBlock = null;
		}

		public void UpdateVisibility(float _fogLightness, FColor _ambient)
		{
			foreach (var point in LiveCoords.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y].UpdateVisibility(_fogLightness, _ambient);
			}
		}
	}
}