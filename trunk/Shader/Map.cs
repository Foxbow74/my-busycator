using System;

namespace Shader
{
	internal class Map
	{
		public const int SIZE = 96;
		private readonly int[,] _map = new int[SIZE,SIZE];
		public static Random rnd = new Random(1);

		public Map()
		{
			for (var i = 0; i < SIZE; ++i)
			{
				for (var j = 0; j < SIZE; ++j)
				{
					_map[i, j] = 0;
				}
			}

			for (int i = 5; i < SIZE - 5; i += 3)
			{
				_map[i, SIZE / 3] = 255;
				_map[SIZE / 3, i] = 255;
			}

			_map[5, 5] = 1;
			_map[5, 6] = 1;
			_map[5, 7] = 1;
			_map[5, 8] = 1;
			_map[5, 9] = 1;
			//_map[7, 7] = 1;

			for (int i = 0; i < (SIZE * SIZE) / 10000; ++i)
			{
				_map[rnd.Next(SIZE), rnd.Next(SIZE)] = rnd.Next(255);
			}
		}

		public int this[int _x, int _y]
		{
			get
			{
				return _map[_x, _y];
			}
		}
	}
}