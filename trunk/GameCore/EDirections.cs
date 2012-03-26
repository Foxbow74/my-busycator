using System;

namespace GameCore
{
	[Flags] public enum EDirections
	{
		NONE = 0x0,
		UP = 0x1,
		DOWN = 0x2,
		LEFT = 0x4,
		RIGHT = 0x8,
		ALL = UP | DOWN | LEFT | RIGHT,
	}
}