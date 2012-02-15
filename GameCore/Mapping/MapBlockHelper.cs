using System;
using System.Linq;
using GameCore.Mapping.Layers;

namespace GameCore.Mapping
{
	internal static class MapBlockHelper
	{
		public static void Clear(MapBlock _block, Random _rnd, WorldLayer _layer)
		{
			var def = _layer.DefaultEmptyTerrains.ToArray();
			for (var i = 0; i < MapBlock.SIZE; ++i)
			{
				for (var j = 0; j < MapBlock.SIZE; ++j)
				{
					_block.Map[i, j] = def[_rnd.Next(def.Length)];
				}
			}
		}
	}
}