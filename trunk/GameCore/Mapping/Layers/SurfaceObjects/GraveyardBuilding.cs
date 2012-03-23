using System;
using System.Linq;
using RusLanguage;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
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
}