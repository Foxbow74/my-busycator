using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public abstract class AbstractGameTest1 : AbstractGameTestX
	{
		static AbstractGameTest1()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 1;
		}
	}
}