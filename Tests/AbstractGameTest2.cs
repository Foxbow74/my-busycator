using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public abstract class AbstractGameTest2 : AbstractGameTestX
	{
		static AbstractGameTest2()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 2;
		}
	}
}