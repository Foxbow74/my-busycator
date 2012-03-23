using System.Linq;
using GameCore.Objects;
using RusLanguage;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	class InnBuilding : Building
	{
		public InnBuilding(City _city)
			: base(_city)
		{
			Sex = ESex.FEMALE;
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.INN; }
		}

		protected override string BuildingName
		{
			get { return "гостиница"; }
		}

		public override uint MaxCountInCity
		{
			get
			{
				return 2;
			}
		}

		public override void Fill(MapBlock _block)
		{
			base.Fill(_block);
			var innerPerimeter = Room.RoomRectangle.Inflate(-1, -1);
			var indoor = InDoorWorldCoords.Select(MapBlock.GetInBlockCoords);
			var doorCoords = MapBlock.GetInBlockCoords(DoorWorldCoords);
			var corners = innerPerimeter.CornerPoints.Except(indoor).OrderBy(_point => _point.GetDistTill(doorCoords)).ToArray();
			var allPoints = innerPerimeter.BorderPoints.Except(corners).Except(indoor).OrderByDescending(_point => _point.GetDistTill(doorCoords)).ToArray();


			var cornerTiles = new[] { ETiles.NONE, ETiles.NONE, ETiles.CHAIR, ETiles.BARREL }.OrderBy(_tiles => World.Rnd.Next()).ToArray();
			var perimeterTiles = new[] { ETiles.NONE, ETiles.CHEST, ETiles.BED, ETiles.ARMOR_RACK, ETiles.BED}.OrderBy(_tiles => World.Rnd.Next()).ToArray();

			for (var index = 0; index < allPoints.Length; index++)
			{
				var point = allPoints[index];
				var tile = perimeterTiles[index % perimeterTiles.Length];
				if (tile != ETiles.NONE)
				{
					_block.AddObject(tile.GetThing(), point);
				}
			}

			for (var index = 0; index < corners.Length; index++)
			{
				var point = corners[index];
				var tile = cornerTiles[index % cornerTiles.Length];
				if (tile != ETiles.NONE)
				{
					_block.AddObject(tile.GetThing(), point);
				}
			}
		}
	}
}