using System;
using GameCore.Essences.Mechanisms;
using GameCore.Essences.Things.LightSources;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Essences.Weapons;
using UnsafeUtils;

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
                case 0:
                    m_blocks[0, 0] = EMapBlockTypes.ETERNAL_SNOW;
			        break;
                case 1:
                case 2:
                    m_blocks[0, 0] = EMapBlockTypes.GROUND;
					break;
			}
		}

		public EMapBlockTypes[,] Generate()
		{
			return m_blocks;
		}

        public static void Fill(MapBlock _block, int _worldSeed, EMapBlockTypes _baseType)
		{
            if (_baseType==EMapBlockTypes.NONE) return;

            switch (Constants.WORLD_SEED)
            {
                case 0:
                    World.TheWorld.Avatar.Layer.Ambient = FColor.Empty;
                    _block.Map[16,16]=ETerrains.GRAY_BRICK_WALL;
                    _block.AddEssence(new IndoorLight(new LightSource(18, new FColor(5f, 1f, 0f, 0f)), EssenceHelper.GetMaterial<CopperMaterial>()), new Point(10, 17));
                    _block.AddEssence(new IndoorLight(new LightSource(18, new FColor(5f, 0f, 1f, 0f)), EssenceHelper.GetMaterial<CopperMaterial>()), new Point(22, 22));
                    _block.AddEssence(new IndoorLight(new LightSource(18, new FColor(5f, 0f, 0f, 1f)), EssenceHelper.GetMaterial<CopperMaterial>()), new Point(22, 10));
                    break;
                case 1:
                    _block.AddEssence(EssenceHelper.GetFirstFoundedThing<ClosedDoor>(), new Point(2, 1));
			        _block.AddEssence(EssenceHelper.GetFirstFoundedThing<ClosedDoor>(), new Point(1, 2));

			        _block.AddEssence(EssenceHelper.GetFirstFoundedItem<Axe>(), new Point(4, 1));
			        _block.AddEssence(EssenceHelper.GetFirstFoundedItem<Axe>(), new Point(3, 2));
                    break;
                case 2:
                    _block.AddEssence(new MagicPlate(EssenceHelper.GetMaterial<CopperMaterial>(), 0, EMagicPlateEffect.RANDOM_MONSTER_APPEAR), new Point(10, 10));
					_block.AddEssence(new Button(EssenceHelper.GetMaterial<BrassMaterial>(), 0), new Point(1, 1));
                    break;
            }
		}
	}
}
