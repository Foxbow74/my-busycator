﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;
using GameCore.Objects.Furniture.LightSources;

namespace GameCore.Mapping.Layers
{
	public class Surface : WorldLayer
	{
		private EMapBlockTypes[,] m_worldMap;
		private WorldMapGenerator m_worldMapGenerator;
		private CityGenerator m_cityGenerator;

		public EMapBlockTypes[,] WorldMap
		{
			get
			{
				if (m_worldMap == null)
				{
					var rnd = new Random(World.WorldSeed);

					m_worldMapGenerator = new WorldMapGenerator(WorldMapSize, rnd);
					m_worldMap = m_worldMapGenerator.Generate();

					m_cityGenerator = new CityGenerator(m_worldMap);
					m_cityGenerator.GenerateCityArea(rnd);
				}
				return m_worldMap;
			}
		}

		public override float GetFogColorMultiplier(LiveMapCell _liveCell)
		{
			return Math.Max(0.6f, 1f - _liveCell.TerrainAttribute.IsPassable);
		}

		public int WorldMapSize
		{
			get { return 30; }// 150; }
		}

		internal override IEnumerable<ETerrains> DefaultEmptySpaces
		{
			get
			{
				yield return ETerrains.GRASS;
			}
		}

		internal override IEnumerable<ETerrains> DefaultWalls
		{
			get { yield return ETerrains.RED_BRICK_WALL; }
		}

		public EMapBlockTypes GetBlockType(Point _blockId)
		{
			return WorldMap[_blockId.X + WorldMapSize/2, _blockId.Y + WorldMapSize/2];
		}


		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);

			var type = GetBlockType(_blockId);
			switch (type)
			{
				case EMapBlockTypes.GROUND:
					MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);
					break;
				case EMapBlockTypes.SEA:
					MapBlockHelper.Clear(block, rnd, this, new []{ETerrains.WATER, });
					break;
				case EMapBlockTypes.CITY:
					MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);
					m_cityGenerator.GenerateCityBlock(rnd, this, _blockId, block);
					break;
				case EMapBlockTypes.NONE:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			//for (var i = 3; i <= 8; i++)
			//{
			//    block.Map[i, 3] = ETerrains.RED_BRICK_WALL;
			//    block.Map[3, i] = ETerrains.RED_BRICK_WALL;
			//    block.Map[i, 8] = ETerrains.RED_BRICK_WALL;
			//    block.Map[8, i] = ETerrains.RED_BRICK_WALL;
			//}
			
			//block.Map[6, 3] = ETerrains.WINDOW;
			//block.Map[3, 6] = ETerrains.WINDOW;
			//block.Map[6, 8] = ETerrains.GROUND;

			//block.AddObject(ETiles.DOOR.GetThing(), new Point(6, 8));
			//block.AddObject(new Sign(ETiles.SWORD, FColor.White, "'Оружейник'"), new Point(7, 8));

			//block.AddObject(new OnWallTorch(new LightSource(15, new FColor(4f,1f,1f,0.5f)), EDirections.DOWN), new Point(7, 9));

			//{
			//    var cnt = rnd.Next(rnd.Next(70));
			//    for (var i = 0; i < cnt; ++i)
			//    {
			//        var x = rnd.Next(MapBlock.SIZE);
			//        var y = rnd.Next(MapBlock.SIZE);
			//        var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
			//        if (attr.IsPassable > 0)
			//        {
			//            block.Map[x, y] = ETerrains.MUSHROOM;
			//        }
			//    }
			//}

			{
				var itmcnt = 20 + rnd.Next(rnd.Next(20));
				for (var i = 0; i < itmcnt; ++i)
				{
					var x = rnd.Next(MapBlock.SIZE);
					var y = rnd.Next(MapBlock.SIZE);

					var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
					if (attr.IsPassable > 0)
					{
						var point = new Point(x, y);
						var any = block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
						var thing = World.Rnd.Next(2) == 0 ? ThingHelper.GetFakedThing(block) : ThingHelper.GetFakedItem(block.RandomSeed);

						if (thing.Is<Stair>() && (x == MapBlock.SIZE - 1 || y == MapBlock.SIZE - 1))
						{
							continue;
						}

						if (thing is Item)
						{
							if (any.Any(_thing => !(_thing is Item)))
							{
								continue;
							}
						}
						else if (any.Any())
						{
							continue;
						}
						block.AddObject(thing, point);
					}
				}
			}

			//block.AddCreature(new Monster(this), new Point(rnd.Next(MapBlock.SIZE), rnd.Next(MapBlock.SIZE)));

			return block;
		}

		public override FColor Ambient
		{
			get
			{
				return new FColor(1f,1f,1f,0.5f).Multiply(0f); 
			}
		}
	}
}