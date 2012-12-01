using System;
using System.Linq;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public class LiveMapBlock
	{
		private readonly Point m_liveCellZero;
		private readonly LiveMap m_liveMap;
		private readonly int m_liveMapBlockIndex;
		private MapBlock m_mapBlock;

		public LiveMapBlock(LiveMap _liveMap, Point _liveMapBlockId, int _liveMapBlockIndex)
		{
			m_liveMap = _liveMap;
			LiveMapBlockId = _liveMapBlockId;
			m_liveMapBlockIndex = _liveMapBlockIndex;
			m_liveCellZero = LiveMapBlockId*Constants.MAP_BLOCK_SIZE;

			foreach (var point in m_liveCellZero.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y] = new LiveMapCell(this, point);
			}
		}

		public MapBlock MapBlock { get { return m_mapBlock; } private set { m_mapBlock = value; } }

		public bool IsBorder { get; set; }

		public IEnumerable<Creature> Creatures
		{
			get
			{
				var arr = MapBlock.Creatures.SelectMany(e => e.Value).ToArray();
				return arr;
			}
		}

		public Point LiveMapBlockId { get; private set; }

		public void ClearTemp()
		{
			foreach (var point in m_liveCellZero.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y].ClearTemp();
			}
		}

		/// <summary>
		/// Мировые координаты левого верхнего угла блока
		/// </summary>
		public Point WorldCoords { get; private set; }

		private void Fill()
		{
			var rnd = new Random(MapBlock.RandomSeed);

			WorldCoords = m_mapBlock.BlockId*Constants.MAP_BLOCK_SIZE;

			foreach (var point in m_liveCellZero.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y].SetMapCell(m_mapBlock, point - m_liveCellZero, (float)rnd.NextDouble(), point, m_liveMap);
			}
			foreach (var tuple in m_mapBlock.Objects)
			{
				var cellId = tuple.Item2 + m_liveCellZero;
				if (tuple.Item1 is Item)
				{
					m_liveMap.Cells[cellId.X, cellId.Y].AddItemIntenal((Item) tuple.Item1);
				}
				else if (tuple.Item1.Is<Thing>())
				{
					m_liveMap.Cells[cellId.X, cellId.Y].Thing = (Thing) tuple.Item1;
				}
			}
			foreach (var pair in m_mapBlock.Creatures)
			{
				foreach (var creature in Creatures)
				{
					creature.LiveCoords = m_liveCellZero + pair.Key;	
				}
				
			}
		}

		public void UpdatePathFinderMapCoords()
		{
			foreach (var point in m_liveCellZero.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y].UpdatePathFinderMapCoord();
			}
		}

		public void SetMapBlock(MapBlock _mapBlock)
		{
			MapBlock = _mapBlock;
			Fill();
		}

		public override string ToString() { return LiveMapBlockId + " MB:" + (m_mapBlock == null ? "<null>" : m_mapBlock.BlockId.ToString()); }

		public void Clear()
		{
			
//			m_liveCellZero

			if (MapBlock == null) return;
			MapBlock = null;
		}

		public void RemoveCreature(Creature _creature, Point _oldLiveCoords) { MapBlock.RemoveCreature(_creature); }

		public void AddCreature(Creature _creature, Point _newLiveCoords)
		{
			MapBlock.AddCreature(_creature, BaseMapBlock.GetInBlockCoords(_newLiveCoords));
		}

		public void UpdateVisibility(float _fogLightness, FColor _ambient)
		{
			foreach (var point in m_liveCellZero.GetAllBlockPoints())
			{
				m_liveMap.Cells[point.X, point.Y].UpdateVisibility(_fogLightness, _ambient);
			}
		}
	}
}