using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using GameCore.Mapping;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameUi.UIBlocks;
using UnsafeUtils;
using GameCore;
using Point = GameCore.Misc.Point;

namespace ConsoleTests
{
	class Program
	{
		const int SIZE = 64;
		static void Main(string[] _args)
		{
			var random = new Random();
			//PerlinTest();
			//GenerateBlockMaps(random);

			foreach (var point in new Point(5, 5).GetSpiral(11))
			{
				Debug.WriteLine(point);
			}
		}

		//private static void GenerateBlockMaps(Random random) {
		//    var map = GenerateWorldMapTest();
		//    var bmp = new Bitmap(MapBlock.SIZE * SIZE, MapBlock.SIZE * SIZE);
		//    var blocks = new Dictionary<Point, MapBlock>();
		//    var blockPoints = new Rct(0, 0, MapBlock.SIZE, MapBlock.SIZE).AllPoints;
		//    var mapPoints = new Rct(0, 0, SIZE, SIZE).AllPoints.OrderBy(_point => random.Next());

		//    var total = mapPoints.Count();
		//    var cur = 0;

		//    var s = "";
		//    foreach (var id in mapPoints)
		//    {
		//        var block = SurfaceBlockGenerator.GenerateBlock(id, map, blocks);
		//        blocks.Add(id, block);

		//        var s1 = string.Format("{0:N0}%", (100.0*++cur/total));
		//        if (s != s1)
		//        {
		//            Debug.WriteLine(s1);
		//            s = s1;
		//        }

		//        var blockId = id*MapBlock.SIZE;

		//        foreach (var pnt in blockPoints)
		//        {
		//            var value = block.Map[pnt.X, pnt.Y];
		//            var fcolor = MiniMapUiBlock.GetColor(TerrainAttribute.GetMapBlockType(value));
		//            var color = Color.FromArgb((int) (fcolor.R*255), (int) (fcolor.G*255), (int) (fcolor.B*255));
		//            bmp.SetPixel(pnt.X + blockId.X, pnt.Y + blockId.Y, color);
		//        }
		//    }
		//    bmp.Save("blocks.bmp");
		//}

		private static EMapBlockTypes[,] GenerateWorldMapTest()
		{
			var random = new Random();
			var mg =new WorldMapGenerator2(SIZE,random);
			var map = mg.CreatePatchMap();
			var bmp = new Bitmap(SIZE, SIZE);

			foreach(var pnt in new Rct(0,0,SIZE,SIZE).AllPoints)
			{
				var value = map[pnt.X,pnt.Y];
				var fcolor = MiniMapUiBlock.GetColor(value);
				var color = Color.FromArgb((int)(fcolor.R * 255), (int)(fcolor.G * 255), (int)(fcolor.B * 255));
				bmp.SetPixel(pnt.X,pnt.Y,color);
			}
			bmp.Save("map.bmp");
			return map;
		}

		private static void PerlinTest() {
			const int size = 128;
			var random = new Random();
			var wmg = new WorldMapGenerator(size, random);
			var map = wmg.Generate();

			var hs = size/2;
			var cntr = new GameCore.Misc.Point(hs, hs);
			for (var k = 0.1f; k < 1f; k += 0.1f)
			{
				var bmp = PerlinNoise.GenerateBitmap(size, size, 0.1f, 1f, k, 9, 4, 0, 255);
				bmp.Save("det_" + k.ToString("N1") + ".bmp");
			}
			for (var k = 1; k < 10; k += 1)
			{
				var bmp = PerlinNoise.GenerateBitmap(size, size, 0.1f, 1f, 1f, k, 4, 0, 255);
				bmp.Save("oct_" + k.ToString("N1") + ".bmp");
				continue;
				var bitmap = new Bitmap(size, size);
				var noise = PerlinNoise.Generate(size, size, 0.1f, 1f, 1f, k, 1);
				for (var i = 0; i < size; ++i)
				{
					for (var j = 0; j < size;++j )
					{
						var ko = (hs - cntr.GetDistTill(new GameCore.Misc.Point(i, j))) / hs  *  Math.Abs(noise[i, j]);
						if (ko > 1) ko = 1;
						if (ko < 0.5) continue;
						
						var rgb = (int) (ko*255);
						bitmap.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
						switch (map[i, j])
						{
							case EMapBlockTypes.NONE:
								break;
							case EMapBlockTypes.GROUND:
							case EMapBlockTypes.FOREST:
							case EMapBlockTypes.SEA:
							case EMapBlockTypes.CITY:
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}


				bitmap.Save("oct_" + k.ToString("N1") + ".bmp");
			}
		}
	}
}
