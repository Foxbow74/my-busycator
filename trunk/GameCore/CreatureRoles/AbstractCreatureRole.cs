using System;
using GameCore.Creatures;
using GameCore.Mapping.Layers.SurfaceObjects;

namespace GameCore.CreatureRoles
{
	public abstract class AbstractCreatureRole
	{
		public abstract string Name { get; }
	}

	public abstract class AbstractCitizenRole:AbstractCreatureRole
	{
		public void SetBuilding(Intelligent _intelligent,Building _building)
		{
			Building = _building;
			_building.SetOwner(_intelligent);
		}

		public Building Building { get; private set; }

		public abstract EBuilding BuildingType { get; }

		public override string ToString()
		{
			return Name + " " + Building;
		}
	}

	class AvatarRole : AbstractCreatureRole
	{
		public override string Name
		{
			get { return "аватар"; }
		}
	}

	class HouseKeeper : AbstractCitizenRole
	{
		public override string Name
		{
			get { return "горожанин"; }
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.HOUSE; }
		}
	}

	class TavernKeeper : AbstractCitizenRole
	{
		public override string Name
		{
			get { return "трактирщик"; }
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.TAVERN; }
		}
	}

	class InnKeeper : AbstractCitizenRole
	{
		public override string Name
		{
			get { return "хозяин постоялого двора"; }
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.INN; }
		}
	}

	class ShopKeeper : AbstractCitizenRole
	{
		public override string Name
		{
			get { return "торговец"; }
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.SHOP; }
		}
	}

	class GraveyardKeeper : AbstractCitizenRole
	{
		//Sexton
		public override string Name
		{
			get { return "могильщик"; }
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.GRAVEYARD; }
		}
	}

	//class Militioner : AbstractCitizenRole
	//{
	//    public override string Name
	//    {
	//        get { return "ополченец"; }
	//    }
	//}

	//class CityGuard : AbstractCitizenRole
	//{
	//    public override string Name
	//    {
	//        get { return "стражник"; }
	//    }
	//}
}
