using System;

namespace GameCore.Messages
{
	[Flags] public enum ESelectItemDialogBehavior
	{
		SELECT_ONE = 1,
		SELECT_MULTIPLE = 2,
		ALLOW_CHANGE_FILTER = 4,
	}
}