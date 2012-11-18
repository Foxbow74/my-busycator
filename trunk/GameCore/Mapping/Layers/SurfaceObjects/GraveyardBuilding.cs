using System;
using System.Linq;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Misc;
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

		    var graves = EssenceHelper.GetAllThings<Grave>().ToArray();

			foreach (var point in Room.RoomRectangle.AllPoints)
			{
				if (rnd.Next(4) == 0 && !InDoorWorldCoords.Contains(point))
				{
					_block.AddEssence(graves.RandomItem(rnd), point);
				}
			}
		}
	}
}