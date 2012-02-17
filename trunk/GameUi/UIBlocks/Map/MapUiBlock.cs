using System.Drawing;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace GameUi.UIBlocks.Map
{
	internal class MapUiBlock : UIBlock
	{
		public MapUiBlock(Rectangle _rectangle)
			: base(_rectangle, null, Color.Black.ToFColor())
		{
			World.TheWorld.LiveMap.SetViewPortSize(new Point(ContentRectangle.Width, ContentRectangle.Height));
			MessageManager.NewWorldMessage += MessageManagerNewWorldMessage;
		}

		public override void  Dispose()
		{
			MessageManager.NewWorldMessage -= MessageManagerNewWorldMessage;
			base.Dispose();
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
			TileHelper.DrawHelper.ClearTiles(Rectangle, FColor.Empty);

			var fogColor = Color.FromArgb(255, 70, 70, 70).ToFColor();

			var layer = World.TheWorld.Avatar.Layer;

			var dPoint = World.TheWorld.LiveMap.GetData();

			var width = ContentRectangle.Width;
			var height = ContentRectangle.Height;

			for (var x = 0; x < width; ++x)
			{
				for (var y = 0; y < height; ++y)
				{
					var pnt = new Point(x + dPoint.X, y + dPoint.Y).Wrap(World.TheWorld.LiveMap.SizeInCells,
					                                                     World.TheWorld.LiveMap.SizeInCells);
					var liveCell = World.TheWorld.LiveMap.Cells[pnt.X, pnt.Y];
					var mapCell = liveCell.MapCell;

					var backgroundColor = BackgroundColor;
					//var lighted = FColor.White.Multiply(liveCell.Visibility.A);// > 0 ? FColor.White : FColor.Black;// liveCell.Lighted.Screen(layer.Ambient).Multiply(liveCell.Visibility.A);
					var lighted = liveCell.Lighted.Screen(layer.Ambient).Multiply(liveCell.Visibility.A);


					var tile = mapCell.Tile.GetTile() ?? mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
					var color = tile.Color.Multiply(lighted).Clamp();
					var lightness = lighted.Lightness();

					if (lightness > 0.3)
					{
						tile.Draw(x + ContentRectangle.Left, y + ContentRectangle.Top, color.Clamp(), backgroundColor);
						liveCell.SetIsSeenBefore();
					}
					else if (liveCell.IsSeenBefore)
					{
						if (mapCell.TerrainAttribute.IsPassable == 1) continue;
						tile = mapCell.Terrain.Tile(mapCell.WorldCoords, mapCell.BlockRandomSeed);
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