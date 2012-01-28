#region

using System;

#endregion

namespace GameCore
{
	public enum EThing
	{
		NONE,
		CHEST,
		DOOR,
		SWORD,
		AXE,
		CROSSBOW
	}

	public static class ItemTipeTileResolver
	{
		public static ETiles Tile(this EThing _thing)
		{
			switch (_thing)
			{
				case EThing.AXE:
					return ETiles.AXE;
				case EThing.SWORD:
					return ETiles.SWORD;
				case EThing.CHEST:
					return ETiles.CHEST;
				case EThing.DOOR:
					return ETiles.DOOR;
				case EThing.CROSSBOW:
					return ETiles.CROSSBOW;
				default:
					throw new ArgumentOutOfRangeException("_thing");
			}
		}
	}
}