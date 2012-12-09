using System;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Creatures
{
	public class CreatureGeoInfo
	{
		private Point m_worldCoords;

		public CreatureGeoInfo(Creature _creature, Point _position)
		{
			Creature = _creature;
			WorldCoords = _position;
		}

		public Creature Creature { get; private set; }
		
		public Point WorldCoords
		{
			get { return m_worldCoords; }
			set
			{
				if (LiveCoords!=null && value != this[0, 0].WorldCoords)
				{
					throw new ApplicationException();
				}
				m_worldCoords = value;
			}
		}

		public Point LiveCoords { get; set; }

		public Point PathMapCoords
		{
			get { return this[0, 0].PathMapCoords; }
		}

		public LiveMapCell this[Point _point]
		{
			get { return World.TheWorld.LiveMap.GetCell(LiveCoords + _point); }
		}

		public LiveMapCell this[int _x, int _y]
		{
			get { return World.TheWorld.LiveMap.GetCell(LiveCoords + new Point(_x, _y)); }
		}

		public WorldLayer Layer { get; set; }

		public void Check()
		{
			if (LiveCoords != null && World.TheWorld.LiveMap.GetCell(LiveCoords).LiveMapBlock.WorldCoords != null && m_worldCoords != this[0, 0].WorldCoords)
			{
				throw new ApplicationException();
			}
		}
	}
}