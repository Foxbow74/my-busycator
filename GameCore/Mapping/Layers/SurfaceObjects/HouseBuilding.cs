using System.Linq;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	internal class HouseBuilding : Building
	{
		public HouseBuilding(City _city)
			: base(_city) { }

		public override EBuilding BuildingType { get { return EBuilding.HOUSE; } }

		protected override string BuildingName { get { return "дом"; } }

		public override uint MinCountInCity { get { return 0; } }

		protected Point BedCoords { get; private set; }

		public override void Fill(MapBlock _block)
		{
			base.Fill(_block);
			var innerPerimeter = Room.RoomRectangle.Inflate(-1, -1);
			var indoor = InDoorWorldCoords.Select(BaseMapBlock.GetInBlockCoords);
			var doorCoords = BaseMapBlock.GetInBlockCoords(DoorWorldCoords);
			var corners = innerPerimeter.CornerPoints.Except(indoor).OrderBy(_point => _point.GetDistTill(doorCoords)).ToArray();
			var allPoints = innerPerimeter.BorderPoints.Except(corners).Except(indoor).OrderByDescending(_point => _point.GetDistTill(doorCoords)).ToArray();


			var cornerTiles = new[] {ETiles.NONE, ETiles.NONE, ETiles.CHAIR, ETiles.BARREL}.OrderBy(_tiles => World.Rnd.Next()).ToArray();
			var perimeterTiles = new[] {ETiles.NONE, ETiles.CHEST, ETiles.WEAPON_RACK, ETiles.ARMOR_RACK, ETiles.CABINET}.OrderBy(_tiles => World.Rnd.Next()).ToArray();
			BedCoords = allPoints[0];
			_block.AddObject(ETiles.BED.GetThing(), BedCoords);
			for (var index = 1; index < allPoints.Length; index++)
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