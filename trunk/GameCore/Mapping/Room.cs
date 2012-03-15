﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public class Room
	{
		public Room(Rct _roomRect, Rct _areaRect, Point _blockId)
		{
			RoomRectangle = _roomRect;
			AreaRectangle = _areaRect;
			BlockId = _blockId;
			ConnectedTo = new List<Room>();
		}

		protected int RandomSeed
		{
			get; private set;
		}

		public Point BlockId
		{
			get; private set; 
		}

		public WorldLayer Layer { get; private set; }

		public Rct RoomRectangle { get; private set; }
		public Rct AreaRectangle { get; private set; }

		public Rct WorldRoomRectangle { get; private set; }

		private bool m_isConnected;
		public bool IsConnected 
		{ 
			get
			{
				return m_isConnected;
			}
			set
			{
				if (m_isConnected == value)
				{
					return;
				}
				if (!value)
				{
					throw new NotImplementedException();
				}
				m_isConnected = true;
				foreach (var room in ConnectedTo)
				{
					room.IsConnected = true;
				}
			}
		}

		internal void Connect(params Room[] _rooms)
		{
			if(!IsConnected)
			{
				IsConnected = _rooms.Any(_room => _room.IsConnected);
			}

			foreach (var room in _rooms.Where(_room => !(_room == this || ConnectedTo.Contains(_room))))
			{
				ConnectedTo.Add(room);
				room.Connect(this);
			}
		}

		public List<Room> ConnectedTo { get; private set; }

		public virtual void AddedToBlock(MapBlock _mapBlock)
		{
			BlockId = _mapBlock.BlockId;
			WorldRoomRectangle = new Rct
				(
					RoomRectangle.Left + BlockId.X * MapBlock.SIZE,
					RoomRectangle.Top + BlockId.Y * MapBlock.SIZE,
					RoomRectangle.Width,
					RoomRectangle.Height);
			RandomSeed = _mapBlock.RandomSeed + RoomRectangle.LeftTop.GetHashCode();
		}
	}
}
