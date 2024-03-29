﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Mapping
{
	public static class MapBlockHelper
	{
		public static void Clear(this BaseMapBlock _block, Random _rnd, WorldLayer _layer, IEnumerable<ETerrains> _defaultTerrains) { Fill(_block, _rnd, _layer, _defaultTerrains, BaseMapBlock.Rect); }

		public static void Fill(this BaseMapBlock _block, Random _rnd, WorldLayer _layer, IEnumerable<ETerrains> _defaultTerrains, Rct _rct)
		{
			var def = _defaultTerrains.ToArray();
			for (var i = 0; i < _rct.Width; ++i)
			{
				for (var j = 0; j < _rct.Height; ++j)
				{
					_block.Map[i + _rct.Left, j + _rct.Top] = def.RandomItem(_rnd);
				}
			}
		}
	}
}