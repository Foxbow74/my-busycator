using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Misc
{
	[Flags]
	public enum EDirections
	{
		UP = 0x1,
		DOWN = 0x2,
		LEFT = 0x4,
		RIGHT = 0x8
	}

	public class LightSource
	{
		private readonly Creature m_creature;
		private static readonly Dictionary<int, LosManager> m_lightManagers = new Dictionary<int, LosManager>();

		private readonly Point m_worldCoords;
		private readonly int m_radius;
		private readonly FColor m_fColor;

		public LightSource(Creature _creature, int _radius, FColor _fColor)
		{
			m_creature = _creature;
			m_radius = _radius;
			m_fColor = _fColor;
		}

		public LightSource(Point _worldCoords, int _radius, FColor _fColor)
		{
			m_worldCoords = _worldCoords;
			m_radius = _radius;
			m_fColor = _fColor;
		}

		public void LightCells(MapCell[,] _mapCells, Point _center)
		{
			if (!m_lightManagers.ContainsKey(m_radius))
			{
				m_lightManagers.Add(m_radius, new LosManager(m_radius));
			}
			var coords = m_creature == null ? m_worldCoords : m_creature.Coords;
			m_lightManagers[m_radius].LightCells(_mapCells, coords - World.TheWorld.Avatar.Coords + _center, m_fColor);
		}
	}
}
