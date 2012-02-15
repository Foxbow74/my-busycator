using System;
using System.Drawing;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks.Map
{
	internal class MapUiBlock : UIBlock
	{
		const float VISIBILITY_THRESHOLD = 33f/255;

		public MapUiBlock(Rectangle _rectangle)
			: base(_rectangle, null, Color.Black.ToFColor())
		{
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRectangle.Width, ContentRectangle.Height));
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		private void MessageManagerNewWorldMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_MOVE:
					BackgroundColor = FColor.Black;// new FColor(1f, World.TheWorld.Avatar.Layer.Ambient.Lerp(FColor.Black, 0.2f));
					break;
				case WorldMessage.EType.TURN:
					Redraw();
					break;
			}
		}

		private void Redraw()
		{
			TileHelper.DrawHelper.ClearTiles(new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height), FColor.Empty);// BackgroundColor.ToGrayScale());

			var fogColor = Color.FromArgb(255, 70, 70, 70).ToFColor();

			var layer = World.TheWorld.Avatar.Layer;

			var dPoint = World.TheWorld.LiveMap.GetData();

			var width = ContentRectangle.Width;
			var height = ContentRectangle.Height;


			//var blocks = layer.GetBlocksNear(World.TheWorld.Avatar.Coords);
			//foreach (var block in blocks)
			//{
			//    foreach (var lightSource in block.Item2.LightSources)
			//    {
			//        lightSource.LightCells(m_mapCells, m_center);
			//    }
			//}
			//World.TheWorld.Avatar.Light.LightCells(m_mapCells, m_center);
			//var block = World.TheWorld.Avatar.MapBlock;
			//m_losManager.LightCells(m_mapCells, block.LightSources[0] + block.BlockId * MapBlock.SIZE - World.TheWorld.Avatar.Coords + m_center););

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var pnt = new Point(x + dPoint.X, y + dPoint.Y).Wrap(World.TheWorld.LiveMap.SizeInCells, World.TheWorld.LiveMap.SizeInCells);
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];
					var mapCell = liveCell.MapCell;

					var backgroundColor = BackgroundColor;
					var lighted = liveCell.Visibility;//.Multiply(mapCell.Lighted);

					var lightness = lighted.Lightness();

					if (lightness > VISIBILITY_THRESHOLD)
					{
						var tile = mapCell.Tile.GetTile() ?? mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);

						var color = tile.Color.Multiply(mapCell.Lighted).Multiply(lighted).Screen(layer.Ambient);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, color, backgroundColor);

						if (!mapCell.IsSeenBefore) mapCell.SetIsSeenBefore();
					}
					else if (mapCell.IsSeenBefore)
					{
						if (mapCell.TerrainAttribute.IsPassable == 1) continue;
						var tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, fogColor, FColor.Black);
						DrawHelper.FogTile(x + ContentRectangle.Left, y + ContentRectangle.Top);
					}
				}
			}
			World.TheWorld.Avatar.Tile.GetTile().Draw(width / 2 + ContentRectangle.Left, height / 2 + ContentRectangle.Top, Color.White.ToFColor(), BackgroundColor);
		}

		public override void DrawFrame()
		{
		}

		public override void DrawBackground()
		{
		}

		public override void DrawContent()
		{
			
		}
	}
}