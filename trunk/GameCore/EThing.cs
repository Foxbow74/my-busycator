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
		AXE
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
				default:
					throw new ArgumentOutOfRangeException("_thing");
			}
		}
	}
}