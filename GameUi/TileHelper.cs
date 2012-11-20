using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Storeable;

namespace GameUi
{
	public static class TileHelper
	{
		static TileHelper()
		{
			AllTiles = new Dictionary<ETileset, TileSet>();
			AllTerrainTilesets = new Dictionary<ETerrains, TileSet>();
		}

		public static Dictionary<ETerrains, TileSet> AllTerrainTilesets { get; private set; }
		public static Dictionary<ETileset, TileSet> AllTiles { get; private set; }

		public static IResourceProvider Rp { get; private set; }
		public static IDrawHelper DrawHelper { get; private set; }

		public static void Init(IResourceProvider _resourceProvider, IDrawHelper _drawHelper)
		{
			Rp = _resourceProvider;
			DrawHelper = _drawHelper;

            Rp.RegisterFont(EFonts.COMMON, Constants.RESOURCES_FONT_FILE, 12);
            Rp.RegisterFont(EFonts.SMALL, Constants.RESOURCES_FONT_FILE, 8);

            AllTiles.Add(ETileset.NONE, new TileSet(Rp.CreateTile(0,0,FColor.Empty)));

			if (_drawHelper!=null && World.XResourceRoot.TileSets.Count > 0)
			{
				foreach (var xTileSet in World.XResourceRoot.TileSets)
				{
					var set = new TileSet();
					AllTiles.Add(xTileSet.Tileset, set);
					var array = xTileSet.Children.OrderBy(_info => _info.Order).ToArray();
					for (int index = 0; index < array.Length; index++)
					{
						var tileInfo = array[index];
						var atile = Rp.CreateTile(tileInfo.Cx, tileInfo.Cy, tileInfo.Color.GetFColor());
						TileInfoProvider.SetOpacity(xTileSet.Tileset, index, tileInfo.Opacity);
						set.AddTile(atile);
					}
				}
				foreach (var xTileSet in World.XResourceRoot.TerrainSets)
				{
					var set = new TileSet();
					AllTerrainTilesets.Add(xTileSet.Terrains, set);
					foreach (var tileInfo in xTileSet.Children.OrderBy(_info => _info.Order))
					{
						set.AddTile(Rp.CreateTile(tileInfo.Cx, tileInfo.Cy, tileInfo.Color.GetFColor()));
					}
				}
			}
			else
			{
                throw new ApplicationException("База ресурсов не содержит информации от тайлах.");
            }
		}

		public static ATile GetTile(this ETileset _tileset, int _index)
		{
			var r = AllTiles[_tileset];
			return r[_index % r.Tiles.Count];
		}

		public static ATile GetTile(this ETerrains _terrains, int _index)
		{
			if(_terrains==ETerrains.NONE)
			{
				return ETileset.NONE.GetTile(0);
			}
			var ts = AllTerrainTilesets[_terrains];
			return ts[_index];
		}
	}
}