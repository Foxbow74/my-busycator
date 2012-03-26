using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

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
			m_liveCellZero = LiveMapBlockId*BaseMapBlock.SIZE;

			for (var i = 0; i < BaseMapBlock.SIZE; i++)
			{
				for (var j = 0; j < BaseMapBlock.SIZE; j++)
				{
					m_liveMap.Cells[i + m_liveCellZero.X, j + m_liveCellZero.Y] = new LiveMapCell(this, m_liveCellZero + new Point(i, j));
				}
			}
		}

		public MapBlock MapBlock { get { return m_mapBlock; } private set { m_mapBlock = value; } }

		public bool IsBorder { get; set; }

		public IEnumerable<Creature> Creatures
		{
			get
			{
				foreach (var creature in MapBlock.Creatures)
				{
					yield return creature.Item1;
				}
			}
		}

		public Point LiveMapBlockId { get; private set; }

		public LiveMapCell this[int _x, int _y] { get { return m_liveMap.Cells[m_liveCellZero.X + _x, m_liveCellZero.Y + _y]; } }

		public void ClearTemp()
		{
			var liveCellZero = LiveMapBlockId*BaseMapBlock.SIZE;
			for (var i = 0; i < BaseMapBlock.SIZE; i++)
			{
				for (var j = 0; j < BaseMapBlock.SIZE; j++)
				{
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].ClearTemp();
				}
			}
		}

		private void Fill()
		{
			var rnd = new Random(MapBlock.RandomSeed);

			var mapCellZero = m_mapBlock.BlockId*BaseMapBlock.SIZE;
			for (var i = 0; i < BaseMapBlock.SIZE; i++)
			{
				for (var j = 0; j < BaseMapBlock.SIZE; j++)
				{
					var ij = new Point(i, j);
					m_liveMap.Cells[m_liveCellZero.X + i, m_liveCellZero.Y + j].SetMapCell(m_mapBlock, ij, mapCellZero + ij, (float) rnd.NextDouble(), m_liveCellZero + ij, m_liveMap);
				}
			}
			foreach (var tuple in m_mapBlock.Objects)
			{
				var cellId = tuple.Item2 + m_liveCellZero;
				if (tuple.Item1 is Item)
				{
					m_liveMap.Cells[cellId.X, cellId.Y].AddItemIntenal((Item) tuple.Item1);
				}
				else if (tuple.Item1.Is<FurnitureThing>())
				{
					m_liveMap.Cells[cellId.X, cellId.Y].Furniture = (FurnitureThing) tuple.Item1;
				}
			}
			foreach (var tuple in m_mapBlock.Creatures)
			{
				var creature = tuple.Item1;
				var point = tuple.Item2;
				creature.LiveCoords = m_liveCellZero + point;
			}
		}

		public void UpdatePathFinderMapCoords()
		{
			for (var i = 0; i < BaseMapBlock.SIZE; i++)
			{
				for (var j = 0; j < BaseMapBlock.SIZE; j++)
				{
					m_liveMap.Cells[m_liveCellZero.X + i, m_liveCellZero.Y + j].UpdatePathFinderMapCoord();
				}
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
			if (MapBlock == null) return;
			MapBlock = null;
		}

		public void RemoveCreature(Creature _creature, Point _oldLiveCoords) { MapBlock.Creatures.Remove(new Tuple<Creature, Point>(_creature, BaseMapBlock.GetInBlockCoords(_oldLiveCoords))); }

		public void AddCreature(Creature _creature, Point _newLiveCoords)
		{
			var tuple = new Tuple<Creature, Point>(_creature, BaseMapBlock.GetInBlockCoords(_newLiveCoords));
			if (!MapBlock.Creatures.Contains(tuple))
			{
				MapBlock.Creatures.Add(tuple);
			}
		}
	}
}