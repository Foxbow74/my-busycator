using System;
using System.Collections.Generic;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	[Flags]
	public enum EBuilding
	{
		HOUSE = 0x1 << 0,
		STORE = 0x1 << 1,
		TAVERN = 0x1 << 2,
		SHOP = 0x1 << 3,
		GRAVEYARD = 0x1 << 4,
		INN = 0x1 << 5,
	}
}