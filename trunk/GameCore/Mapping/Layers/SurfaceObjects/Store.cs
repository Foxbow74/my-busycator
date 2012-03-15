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
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.INN; }
		}

		protected override string BuildingName
		{
			get { return "постоялый двор"; }
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
	}
}

