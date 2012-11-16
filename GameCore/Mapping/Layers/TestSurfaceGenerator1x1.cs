using System;
using GameCore.Misc;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Essences.Weapons;

namespace GameCore.Mapping.Layers
{
	class TestSurfaceGenerator1x1
	{
		private readonly Random m_rnd;
		private EMapBlockTypes[,] m_blocks = new EMapBlockTypes[1,1];
		public TestSurfaceGenerator1x1(Random _rnd)
		{
			m_rnd = _rnd;
			m_blocks = new EMapBlockTypes[1, 1];
			switch (Constants.WORLD_SEED)
			{
				case 1:
					Generate1();
					break;
			}
		}

		private void Generate1()
		{
			m_blocks[0,0] = EMapBlockTypes.GROUND;

		}

		public EMapBlockTypes[,] Generate()
		{
			return m_blocks;
		}

		public static void Fill(MapBlock _block, int _worldSeed)
		{
			_block.AddEssence(EssenceHelper.GetFirstFoundedThing<ClosedDoor>(), new Point(2, 1));
			_block.AddEssence(EssenceHelper.GetFirstFoundedThing<ClosedDoor>(), new Point(1, 2));

			_block.AddEssence(EssenceHelper.GetFirstFoundedItem<Axe>(), new Point(4, 1));
			_block.AddEssence(EssenceHelper.GetFirstFoundedItem<Axe>(), new Point(3, 2));
		}
	}
}
