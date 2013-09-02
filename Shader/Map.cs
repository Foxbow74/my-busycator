using System;

namespace Shader
{
	internal class Map
	{
		public const int SIZE = 150;
		private readonly int[,] _map = new int[SIZE,SIZE];
		public static Random rnd = new Random(1);

		public Map()
		{
			for (int i = 0; i < SIZE; ++i)
			{
				for (int j = 0; j < SIZE; ++j)
				{
					_map[i, j] = 0;
				}
			}

			_map[5, 5] = 1;
			_map[5, 6] = 1;
			_map[5, 7] = 1;
			_map[5, 8] = 1;
			_map[5, 9] = 1;
			_map[7, 7] = 1;

			for (int i = 0; i < (SIZE * SIZE) / 10; ++i)
			{
				_map[rnd.Next(SIZE), rnd.Next(SIZE)] = 1;
			}
		}

		public int this[int x, int y]
		{
			get
			{
				return _map[x, y];
			}
		}
	}
}