namespace GameCore.CreatureRoles
{
	public abstract class AbstractCreatureRole
	{
		public abstract string Name { get; }
	}

	class AvatarRole : AbstractCreatureRole
	{
		public override string Name
		{
			get { return "аватар"; }
		}
	}

	class HouseKeeper:AbstractCreatureRole
	{
		public override string Name
		{
			get { return "горожанин"; }
		}
	}

	class TavernKeeper:AbstractCreatureRole
	{
		public override string Name
		{
			get { return "трактирщик"; }
		}
	}

	class InnKeeper : AbstractCreatureRole
	{
		public override string Name
		{
			get { return "хозяин постоялого двора"; }
		}
	}

	class ShopKeeper:AbstractCreatureRole
	{
		public override string Name
		{
			get { return "торговец"; }
		}
	}

	class GraveyardKeeper:AbstractCreatureRole
	{
		//Sexton
		public override string Name
		{
			get { return "могильщик"; }
		}
	}

	class Militioner:AbstractCreatureRole
	{
		public override string Name
		{
			get { return "ополченец"; }
		}
	}

	class CityGuard:AbstractCreatureRole
	{
		public override string Name
		{
			get { return "стражник"; }
		}
	}
}
