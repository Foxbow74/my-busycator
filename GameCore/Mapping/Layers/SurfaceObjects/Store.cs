using System;
using System.Linq;
using GameCore.Misc;
using GameCore.Objects;
using RusLanguage;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	class ShopBuilding:Building
	{
		public ShopBuilding(City _city)
			: base(_city)
		{
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.SHOP; }
		}

		protected override string BuildingName
		{
			get { return "магазин"; }
		}

		public override uint MaxCountInCity
		{
			get
			{
				return 4;
			}
		}
	}

	class TavernBuilding : Building
	{
		public TavernBuilding(City _city)
			: base(_city)
		{
			Sex = ESex.FEMALE;
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.TAVERN; }
		}

		protected override string BuildingName
		{
			get { return "таверна"; }
		}

		public override uint MaxCountInCity
		{
			get
			{
				return 2;
			}
		}
	}


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
	}

	class GraveyardBuilding:Building
	{
		public GraveyardBuilding(City _city)
			: base(_city)
		{
			Sex = ESex.IT;
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.GRAVEYARD;}
		}

		protected override string BuildingName
		{
			get { return "кладбище"; }
		}

		public override uint MaxCountInCity
		{
			get
			{
				return 1;
			}
		}

		public override void Fill(MapBlock _block)
		{
			var rnd = new Random(_block.RandomSeed);

			InDoorWorldCoords = Room.RoomRectangle.Inflate(-1, -1).BorderPoints.ToArray();
			OutDoorWorldCoords = Room.RoomRectangle.BorderPoints.ToArray();

			foreach (var point in Room.RoomRectangle.AllPoints)
			{
				if (rnd.Next(5) == 0 && !InDoorWorldCoords.Contains(point))
				{
					_block.Map[point.X, point.Y] = ETerrains.GRAVE;
				}
				else
				{
					_block.Map[point.X, point.Y] = ETerrains.GRASS;
				}
			}
		}
	}

	class HouseBuilding:Building
	{
		public HouseBuilding(City _city)
			: base(_city)
		{
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.HOUSE;}
		}

		protected override string BuildingName
		{
			get { return "дом"; }
		}

		public override uint MinCountInCity
		{
			get
			{
				return 0;
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
			var perimeterTiles = new[] { ETiles.NONE, ETiles.CHEST, ETiles.WEAPON_RACK, ETiles.ARMOR_RACK, ETiles.CABINET }.OrderBy(_tiles => World.Rnd.Next()).ToArray();
			BedCoords = allPoints[0];
			_block.AddObject(ETiles.BED.GetThing(), BedCoords);
			for (var index = 1; index < allPoints.Length; index++)
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

		protected Point BedCoords { get; private set; }
	}
}

