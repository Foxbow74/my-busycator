#region

using System;

#endregion

namespace GameCore
{
	public enum EItems
	{
		NONE,
		CHEST,
		DOOR,
		SWORD,
		AXE
	}

	public static class ItemTipeTileResolver
	{
		public static ETiles Tile(this EItems _item)
		{
			switch (_item)
			{
				case EItems.AXE:
					return ETiles.AXE;
				case EItems.SWORD:
					return ETiles.SWORD;
				case EItems.CHEST:
					return ETiles.CHEST;
				case EItems.DOOR:
					return ETiles.DOOR;
				default:
					throw new ArgumentOutOfRangeException("_item");
			}
		}
	}
}