using System.Linq;
using GameCore.Objects;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	internal class ShopBuilding : Building
	{
		public ShopBuilding(City _city)
			: base(_city) { }

		public override EBuilding BuildingType { get { return EBuilding.SHOP; } }

		protected override string BuildingName { get { return "магазин"; } }

		public override uint MaxCountInCity { get { return 4; } }

		public override void Fill(MapBlock _block, WorldLayer _layer)
		{
			base.Fill(_block, _layer);
			var innerPerimeter = Room.RoomRectangle.Inflate(-1, -1);
			var indoor = InDoorWorldCoords.Select(BaseMapBlock.GetInBlockCoords);
			var doorCoords = BaseMapBlock.GetInBlockCoords(DoorWorldCoords);
			var corners = innerPerimeter.CornerPoints.Except(indoor).OrderBy(_point => _point.GetDistTill(doorCoords)).ToArray();
			var allPoints = innerPerimeter.BorderPoints.Except(corners).Except(indoor).OrderByDescending(_point => _point.GetDistTill(doorCoords)).ToArray();


			var cornerTiles = new[] {ETiles.NONE, ETiles.CABINET, ETiles.TABLE, ETiles.BARREL}.OrderBy(_tiles => World.Rnd.Next()).ToArray();
			var perimeterTiles = new[] {ETiles.NONE, ETiles.CHEST, ETiles.TABLE, ETiles.ARMOR_RACK, ETiles.WEAPON_RACK}.OrderBy(_tiles => World.Rnd.Next()).ToArray();

			for (var index = 0; index < allPoints.Length; index++)
			{
				var point = allPoints[index];
				var tile = perimeterTiles[index%perimeterTiles.Length];
				if (tile != ETiles.NONE)
				{
					_block.AddObject(tile.GetThing(), point);
				}
			}

			for (var index = 0; index < corners.Length; index++)
			{
				var point = corners[index];
				var tile = cornerTiles[index%cornerTiles.Length];
				if (tile != ETiles.NONE)
				{
					_block.AddObject(tile.GetThing(), point);
				}
			}
		}
	}
}