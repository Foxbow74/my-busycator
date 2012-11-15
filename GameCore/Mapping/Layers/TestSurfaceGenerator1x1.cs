using System;
using System.Linq;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furnitures;
using GameCore.Objects.Weapons;

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
			_block.AddObject(ThingHelper.AllFakedFurniture().First(_furniture => _furniture.Is<ClosedDoor>()), new Point(2, 1));
			_block.AddObject(ThingHelper.AllFakedFurniture().First(_furniture => _furniture.Is<ClosedDoor>()), new Point(1, 2));

			_block.AddObject(ThingHelper.AllFakedItems().First(_item => _item.Is<Axe>()), new Point(4, 1));
			_block.AddObject(ThingHelper.AllFakedItems().First(_item => _item.Is<Axe>()), new Point(3, 2));
		}
	}
}
