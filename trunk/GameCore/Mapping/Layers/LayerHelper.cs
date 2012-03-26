using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;

namespace GameCore.Mapping.Layers
{
	public static class LayerHelper
	{
		private const int MIN_ROOM_SIZE = 5;
		private const int MIN_ROOM_SQUARE = 42;
		private const int MAX_DIV_SIZE = 15;

		public static IEnumerable<Room> GenerateRooms(Random _random, Rct _rct, ICollection<Point> _objects, Point _blockId)
		{
			var ableVert = _rct.Width - MIN_ROOM_SIZE*2;
			var ableHor = _rct.Height - MIN_ROOM_SIZE*2;

			if ((ableHor > 1 || ableVert > 1) && (_rct.Width*_rct.Height < MIN_ROOM_SQUARE || _rct.Width > MAX_DIV_SIZE || _rct.Height > MAX_DIV_SIZE || _random.Next(_rct.Width + _rct.Height) > MIN_ROOM_SIZE))
			{
				var divVert = 0;
				var divHor = 0;
				while (divVert == divHor)
				{
					divVert = ableVert > 0 ? _random.Next(ableVert + 1) : 0;
					divHor = ableHor > 0 ? _random.Next(ableHor + 1) : 0;
				}
				var rects = new List<Rct>();
				if (divVert > divHor)
				{
					int vert;
					do
					{
						vert = MIN_ROOM_SIZE + _random.Next(ableVert);
						var val = vert;
						if (_objects.All(_point => _point.X != (_rct.Left + val))) break;
					} while (true);
					rects.Add(new Rct(_rct.Left, _rct.Top, vert, _rct.Height));
					rects.Add(new Rct(_rct.Left + vert + 1, _rct.Top, _rct.Width - (vert + 1), _rct.Height));
				}
				else
				{
					int hor;
					do
					{
						hor = MIN_ROOM_SIZE + _random.Next(ableHor);
						var val = hor;
						if (_objects.All(_point => _point.Y != (_rct.Top + val))) break;
					} while (true);
					rects.Add(new Rct(_rct.Left, _rct.Top, _rct.Width, hor));
					rects.Add(new Rct(_rct.Left, _rct.Top + hor + 1, _rct.Width, _rct.Height - (hor + 1)));
				}
				foreach (var rct in rects)
				{
					if (rct.Width > _rct.Width || rct.Height > _rct.Height)
					{
						throw new ApplicationException("Доля больше чем место под нее");
					}

					foreach (var room in GenerateRooms(_random, rct, _objects, _blockId))
					{
						yield return room;
					}
				}
				yield break;
			}
			yield return MakeRoom(_rct, _random, _objects, _blockId);
		}

		private static Room MakeRoom(Rct _rct, Random _random, ICollection<Point> _objects, Point _blockId)
		{
			var contains = _objects.Where(_rct.Contains).ToArray();
			var size = new Point(MIN_ROOM_SIZE + _random.Next(_rct.Width - MIN_ROOM_SIZE), MIN_ROOM_SIZE + _random.Next(_rct.Height - MIN_ROOM_SIZE));
			for (;;)
			{
				var xy = new Point(_random.Next(_rct.Width - size.X + 1), _random.Next(_rct.Height - size.Y + 1));
				var rect = new Rct(_rct.LeftTop + xy, size.X, size.Y);
				if (!contains.Any() || contains.All(rect.Contains))
				{
					foreach (var contain in contains)
					{
						_objects.Remove(contain);
					}
					return new Room(rect, _rct, _blockId);
				}
			}
		}

		public static IEnumerable<Point> GetRandomPoints(Point _center, Random _rnd, EMapBlockTypes[,] _map, int _size, EMapBlockTypes _set, EMapBlockTypes _empty)
		{
			var tries = 10;
			var list = new List<Point> {_center};
			do
			{
				var point = list[_rnd.Next(list.Count)];
				var add = Add(point, _map, ref _size, _set, _rnd, _empty);
				if (add.Any())
				{
					tries = 10;
				}
				else
				{
					tries--;
				}
				list.AddRange(add);
			} while (_size > 0 && tries > 0);
			return list.Distinct();
		}

		private static IEnumerable<Point> Add(Point _xy, EMapBlockTypes[,] _map, ref int _size, EMapBlockTypes _set, Random _rnd, EMapBlockTypes _empty)
		{
			var list = new List<Point>();
			if (_map[_xy.X, _xy.Y] == _empty)
			{
				list.Add(_xy);
				_map[_xy.X, _xy.Y] = _set;
				if (_size == 0 || _rnd.NextDouble() < 0.1)
				{
					return list;
				}
				_size--;
			}
			var dirs = _rnd.GetRandomDirections();

			foreach (var dir in dirs.AllDirectionsIn())
			{
				var xy = _xy + dir.GetDelta();
				if (_map.GetLength(0) <= xy.X || xy.X < 0) continue;
				if (_map.GetLength(1) <= xy.Y || xy.Y < 0) continue;

				if (_map[xy.X, xy.Y] == _empty)
				{
					list.AddRange(Add(xy, _map, ref _size, _set, _rnd, _empty));
				}
			}
			return list;
		}
	}
}