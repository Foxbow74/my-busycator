using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Mapping.Layers.SurfaceObjects;
using RusLanguage;

namespace GameCore.CreatureRoles
{
	public abstract class AbstractCreatureRole
	{
		public abstract string Name { get; }
		public abstract IEnumerable<FColor> Colors { get; }

		public virtual string this[ESex _sex]
		{
			get
			{
				return Sklonenia.ToSex(Name, _sex);
			}
		}
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

		public override string this[ESex _sex]
		{
			get
			{
				var result = base[_sex] + " " + Building[EPadej.ROD, false];
				return result;
			}
		}
	}

	class AvatarRole : AbstractCreatureRole
	{
		public override string Name
		{
			get { return "аватар"; }
		}

		public override IEnumerable<FColor> Colors { get { throw new NotImplementedException(); } }
	}

	class HouseKeeper : AbstractCitizenRole
	{
		public override string Name
		{
			get { return "владелец"; }
		}

		public override IEnumerable<FColor> Colors
		{
			get
			{
				yield return FColor.DarkGray;
				yield return FColor.Gray;
				yield return FColor.LightGray;
				yield return FColor.LightSlateGray;
			}
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

		public override IEnumerable<FColor> Colors
		{
			get
			{
				yield return FColor.GreenCopper;
				yield return FColor.GreenLeaf;
				yield return FColor.GreenWaterloo;
			}
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
			get { return "хозяин"; }
		}

		public override EBuilding BuildingType
		{
			get { return EBuilding.INN; }
		}

		public override IEnumerable<FColor> Colors
		{
			get
			{
				yield return FColor.YellowMetal;
				yield return FColor.YellowSea;
				yield return FColor.YellowGreen;
			}
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


		public override IEnumerable<FColor> Colors
		{
			get
			{
				yield return FColor.BlueWhale;
				yield return FColor.SkyBlue;
				yield return FColor.BlueChalk;
			}
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


		public override IEnumerable<FColor> Colors
		{
			get
			{
				yield return FColor.BlackMagic;
				yield return FColor.BlackPearl;
				yield return FColor.BlackHaze;
			}
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
