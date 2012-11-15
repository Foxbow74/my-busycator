using System;
using System.Linq;
using GameCore.Objects;
using GameCore.Objects.Furnitures;
using RusLanguage;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	internal class GraveyardBuilding : Building
	{
		public GraveyardBuilding(City _city)
			: base(_city) { Sex = ESex.IT; }

		public override EBuilding BuildingType { get { return EBuilding.GRAVEYARD; } }

		protected override string BuildingName { get { return "кладбище"; } }

		public override uint MaxCountInCity { get { return 1; } }

		public override void Fill(MapBlock _block, WorldLayer _layer)
		{
			var rnd = new Random(_block.RandomSeed);

			InDoorWorldCoords = Room.RoomRectangle.Inflate(-1, -1).BorderPoints.ToArray();
			OutDoorWorldCoords = Room.RoomRectangle.BorderPoints.ToArray();

			MapBlockHelper.Fill(_block, rnd, _layer, _layer.DefaultEmptySpaces, Room.AreaRectangle);

		    var graves = ThingHelper.AllFakedFurniture().Where(_ff => _ff.Is<Grave>()).ToArray();

			foreach (var point in Room.RoomRectangle.AllPoints)
			{
				if (rnd.Next(4) == 0 && !InDoorWorldCoords.Contains(point))
				{
					_block.AddObject(graves[rnd.Next(graves.Length)], point);
				}
			}
		}
	}
}