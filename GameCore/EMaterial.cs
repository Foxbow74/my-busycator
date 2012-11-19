﻿using System;

namespace GameCore
{
	[Flags] public enum EMaterial
	{
		METAL = 0x1 << 0,
		WOOD = 0x1 << 1,
		MINERAL = 0x1 << 2,
		FLASH = 0x1 << 3,
		SHRUB = 0x1 << 4,
		MUSHRUM = 0x1 << 5,
	}
}