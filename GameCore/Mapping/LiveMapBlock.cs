﻿using System;
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

			for (var i = 0; i < Constants.MAP_BLOCK_SIZE; i++)
			{
				for (var j = 0; j < Constants.MAP_BLOCK_SIZE; j++)
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
				foreach (var creaturePosition in MapBlock.Creatures)
				{
					yield return creaturePosition.Creature;
				}
			}
		}

		public Point LiveMapBlockId { get; private set; }

		public LiveMapCell this[int _x, int _y] { get { return m_liveMap.Cells[m_liveCellZero.X + _x, m_liveCellZero.Y + _y]; } }

		public void ClearTemp()
		{
			var liveCellZero = LiveMapBlockId*Constants.MAP_BLOCK_SIZE;
			for (var i = 0; i < Constants.MAP_BLOCK_SIZE; i++)
			{
				for (var j = 0; j < Constants.MAP_BLOCK_SIZE; j++)
				{
					m_liveMap.Cells[liveCellZero.X + i, liveCellZero.Y + j].ClearTemp();
				}
			}
		}

		private void Fill()
		{
			var rnd = new Random(MapBlock.RandomSeed);

			var mapCellZero = m_mapBlock.BlockId*Constants.MAP_BLOCK_SIZE;
			for (var i = 0; i < Constants.MAP_BLOCK_SIZE; i++)
			{
				for (var j = 0; j < Constants.MAP_BLOCK_SIZE; j++)
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
				else if (tuple.Item1.Is<Thing>())
				{
					m_liveMap.Cells[cellId.X, cellId.Y].Thing = (Thing) tuple.Item1;
				}
			}
			foreach (var creaturePosition in m_mapBlock.Creatures)
			{
				creaturePosition.Creature.LiveCoords = m_liveCellZero + creaturePosition.Position;
			}
		}

		public void UpdatePathFinderMapCoords()
		{
			for (var i = 0; i < Constants.MAP_BLOCK_SIZE; i++)
			{
				for (var j = 0; j < Constants.MAP_BLOCK_SIZE; j++)
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

		public void RemoveCreature(Creature _creature, Point _oldLiveCoords) { MapBlock.RemoveCreature(_creature); }

		public void AddCreature(Creature _creature, Point _newLiveCoords)
		{
			MapBlock.AddCreature(_creature, BaseMapBlock.GetInBlockCoords(_newLiveCoords));
		}

		public void UpdateVisibility(float _fogLightness, FColor _ambient)
		{
			for (var i = 0; i < Constants.MAP_BLOCK_SIZE; i++)
			{
				for (var j = 0; j < Constants.MAP_BLOCK_SIZE; j++)
				{
					this[i, j].UpdateVisibility(_fogLightness, _ambient);
				}
			}
		}
	}
}