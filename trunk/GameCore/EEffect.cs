using System;

namespace GameCore
{
	[Flags] public enum EEffect
	{
		HEAL = 0x1 << 0,
		ATTACK = 0x1 << 1,
		DEFENCE = 0x1 << 2,
		ATTR = 0x1 << 3,
		SKILL = 0x1 << 4,
		SELF = 0x1 << 5,
		GOOD = 0x1 << 6,
		BAD = 0x1 << 7,
		DISTANCE = 0x1 << 8,
	}
}