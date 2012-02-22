using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore.Mapping.Layers;

namespace GameCore.Mapping
{
	internal static class MapBlockHelper
	{
		public static void Clear(MapBlock _block, Random _rnd, WorldLayer _layer, IEnumerable<ETerrains> _defaultTerrains)
		{
			Fill(_block, _rnd, _layer, _defaultTerrains, MapBlock.Rect);
		}

		public static void Fill(MapBlock _block, Random _rnd, WorldLayer _layer, IEnumerable<ETerrains> _defaultTerrains, Rectangle _rectangle)
		{
			var def = _defaultTerrains.ToArray();
			for (var i = 0; i < _rectangle.Width; ++i)
			{
				for (var j = 0; j < _rectangle.Height; ++j)
				{
					_block.Map[i + _rectangle.Left, j + _rectangle.Top] = def[_rnd.Next(def.Length)];
				}
			}
		}
	}
}