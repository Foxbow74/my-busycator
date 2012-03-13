﻿namespace GameCore.Mapping.Layers.SurfaceObjects
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
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.TAVERN; }
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
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.GRAVEYARD;}
		}

		public override uint MaxCountInCity
		{
			get
			{
				return 1;
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
	}
}
