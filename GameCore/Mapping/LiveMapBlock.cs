using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Mapping
{
	public class LiveMapBlock
	{
		private readonly LiveMap m_liveMap;
		private readonly int m_liveMapBlockIndex;
		private MapBlock m_mapBlock;
		private readonly Point m_liveCellZero;
		private readonly List<Creature> m_creatures = new List<Creature>();

		public LiveMapBlock(LiveMap _liveMap, Point _liveMapBlockId, int _liveMapBlockIndex)
		{
			m_liveMap = _liveMap;
			LiveMapBlockId = _liveMapBlockId;
			m_liveMapBlockIndex = _liveMapBlockIndex;
			m_liveCellZero = LiveMapBlockId * MapBlock.SIZE;

			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					m_liveMap.Cells[i + m_liveCellZero.X, j + m_liveCellZero.Y] = new LiveMapCell(this, m_liveCellZero + new Point(i,j));
				}
			}
		}

		public MapBlock MapBlock
		{
			get
			{
				return m_mapBlock;
			}
			private set
			{
				m_mapBlock = value;
			}
		}

		public bool IsBorder { get; set; }

		public IEnumerable<Creature> Creatures
		{
			get { return m_creatures; }
		}

		public Point LiveMapBlockId { get; private set; }

		public void ClearTemp()
		{
			var liveCellZero = LiveMapBlockId * MapBlock.SIZE;
			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].ClearTemp();
				}
			}
		}

		private void Fill()
		{
			var rnd = new Random(MapBlock.RandomSeed);

			var mapCellZero = m_mapBlock.BlockId*MapBlock.SIZE;
			for (var i = 0; i < MapBlock.SIZE; i++)
			{
				for (var j = 0; j < MapBlock.SIZE; j++)
				{
					var ij = new Point(i, j);
					m_liveMap.Cells[m_liveCellZero.X + i, m_liveCellZero.Y + j].SetMapCell(m_mapBlock, ij, mapCellZero + ij, (float)rnd.NextDouble(), m_liveCellZero + ij, m_liveMap);
				}
			}
			foreach (var tuple in m_mapBlock.Objects)
			{
				var cellId = tuple.Item2 + m_liveCellZero;
				if (tuple.Item1 is Item)
				{
					m_liveMap.Cells[cellId.X, cellId.Y].AddItemIntenal((Item) tuple.Item1);
				}
				else if (tuple.Item1.IsFurniture())
				{
					m_liveMap.Cells[cellId.X, cellId.Y].Furniture = tuple.Item1;
				}
			}
			foreach (var tuple in m_mapBlock.Creatures)
			{
				var creature = tuple.Item1;
				var point = tuple.Item2;
				creature.LiveCoords = m_liveCellZero + point;
			}
		}

		public void SetMapBlock(MapBlock _mapBlock)
		{
			MapBlock = _mapBlock;
			Fill();
		}

		public override string ToString()
		{
			return LiveMapBlockId + " MB:" + (m_mapBlock==null?"<null>":m_mapBlock.BlockId.ToString());
		}

		public void Clear()
		{
			if (MapBlock==null) return;

			MapBlock.Creatures.Clear();
			foreach (var creature in m_creatures)
			{
				MapBlock.AddCreature(creature, MapBlock.GetInBlockCoords(creature.LiveCoords));
				creature.LiveCoords = null;
			}
			MapBlock = null;
			m_creatures.Clear();
		}

		public void RemoveCreature(Creature _creature)
		{
			if (!m_creatures.Remove(_creature))
			{
				throw new ApplicationException();
			}
		}

		public void AddCreature(Creature _creature)
		{
			if (m_creatures.Contains(_creature))
			{
				throw new ApplicationException();
			}
			m_creatures.Add(_creature);
		}
	}
}