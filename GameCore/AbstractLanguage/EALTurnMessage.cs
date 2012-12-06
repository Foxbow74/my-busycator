namespace GameCore.AbstractLanguage
{
	public enum EALTurnMessage
	{
		CREATURE_CLOSES_IT,
		CREATURE_OPENS_IT,
		CREATURE_DROPS_IT,
		CREATURE_TAKES_IT,
		CONTAINER_IS_EMPTY,
		CREATURE_DRINKS_IT,
		CREATURE_LIGHT_OFF_IT,
		CREATURE_LIGHT_ON_IT,
		CREATURE_KILLED,
		CREATURE_TAKES_DAMAGE,
		AVATAR_IS_LUCK,
		CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK,
		CREATURES_ATTACK_DAMAGE_IS_ZERO,
		CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK,
		CREATURES_ATTACK_DAMAGE_ADSORBED,
		CREATURE_NOW_STAY_ON,
		CELL_IS_OCCUPIED_BY
	}

	public enum EALVerbs
	{
		HURT,
		HACK,
		MANGLE,
		MISS,
		HIT,
		DONT_HIT,
		FINISH,
		STIKE
	}

	public enum EALSentence
	{
		NONE,
		TACTICK_CHANGED,
		GENERAL,
		THERE_ARE,
		THERE_ARE_WERE,
	}

	public enum EALConst
	{
		EXIT,
		PLEASE_CHOOSE_DIRECTION,
		AN_HELP,
		AN_INTERRACT_WITH_ESSENCE,
		AN_TACTIC_NORMAL,
		AN_DESCEND,
		AN_WORLD_MAP,
		AN_OPEN,
		AN_WAIT,
		AN_SHOOT,
		AN_QUIT,
		AN_INVENTORY,
		AN_USE,
		AN_MOVE_TO,
		AN_ASCEND,
		AN_LOOK_AT,
		AN_ATACK,
		AN_LEAVE_BUILDING,
		AN_CLOSE,
		AN_TACTIC_COWARD,
		AN_TACTIC_BERSERK,
		AN_TAKE,
		AN_DROP,
		AN_DRINK,
		AN_MOVE,
		AN_MOVE_ARROWS
	}

	public enum EALNouns
	{
		StackOf,
		Grave,
		Chair,
		Cabinet,
		ArmorRack,
		WeaponRack,
		Barrel,
		Stair,
		StairUp,
		StairDown,
		Table,
		Bed,
		Door,
		ClosedDoor,
		BackPack,
		Chest,
		Button,
		Lever,
		Ring,
		Potion,
		Corpse,
		IndoorLight,
		Torch,
		Shrub,
		Mushrum,
		MagicPlate,
		Avatar,
		Ctitzen,
		NONE,
		Tree,
		Sign,
		Maple,
		Willow,
		Walnut,
		Spruce,
		Pine,
		Oak,
		Ash,
		Mushrum0,
		Shrub0,
		Crossbow,
		Jaws,
		Bolt,
		Rat,
		Spider,
		Wolf,
		Sword
	}
}