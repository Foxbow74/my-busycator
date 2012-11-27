using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Misc;
using UnsafeUtils;

namespace ConsoleTests
{
	struct Tuple
	{
		public int Item1;
		public int Item2;

		public Tuple(int _i, int _i1)
		{
			Item1 = _i;
			Item2 = _i1;
		}
	}

	class Program
	{
		const int SIZE = 64;
		static void Main(string[] _args)
		{

			var lst = new List<Tuple>();
			var ll = 10;
			for (var i = -ll; i < ll; ++i)
			{
				for (var j = -ll; j < ll; ++j)
				{
					lst.Add(new Tuple(i, j));
				}
			}


			var faPoints = new List<Point>();


			{
				for (var k = 0; k < 20; ++k)
				{
					foreach (var tuple in lst)
					{
						faPoints.Add(new Point(tuple.Item1, tuple.Item2));
					}

					using (new Profiler("Point"))
					{
						int ee = 0;

						foreach (var point in faPoints)
						{
							foreach (var pnt in faPoints)
							{
								if (pnt == point)
								{
									ee++;
								}
							}
						}
					}

					Debug.WriteLine(k + "(1)");
					using (new Profiler("LPoint"))
					{
						int ee = 0;

						foreach (var VARIABLE in faPoints.SelectMany(point=>faPoints.Where(_point => _point == point)))
						{
							ee++;
						}
						
					}
				}
			}

			Profiler.Report();
			
		}
	}


}

