using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameUi.UIBlocks;
using UnsafeUtils;
using GameCore;

namespace ConsoleTests
{
	class Program
	{
		static void Main(string[] _args) 
		{
			//PerlinTest();

			const int size = 256;
			var random = new Random();
			var mg =new WorldMapGenerator2(size,random);
			var map = mg.CreatePatchMap();
			var bmp = new Bitmap(size, size);

			var colors = new Dictionary<ushort, Color>();

			

			foreach(var pnt in new Rct(0,0,size,size).AllPoints)
			{
				var value = map[pnt.X,pnt.Y];
				var fcolor = MiniMapUiBlock.GetColor(value);
				var color = Color.FromArgb((int)(fcolor.R * 255), (int)(fcolor.G * 255), (int)(fcolor.B * 255));
				bmp.SetPixel(pnt.X,pnt.Y,color);
			}
			bmp.Save("map.bmp");
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
