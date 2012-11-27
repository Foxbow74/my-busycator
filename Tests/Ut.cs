using System;

namespace Tests
{
	public static class Ut
	{
		public static void Repeat(this int _count, Action _action)
		{
			for (var i = 0; i < _count; i++)
			{
				_action();
			}
		}
	}
}