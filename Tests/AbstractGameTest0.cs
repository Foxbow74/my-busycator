using System.Diagnostics;
using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public abstract class AbstractGameTest0 : AbstractGameTestX
	{
		static AbstractGameTest0()
		{
			Constants.WORLD_MAP_SIZE = 1;
			Constants.WORLD_SEED = 0;
		}
	}
}