using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;

namespace GameCore.Mapping.Layers
{
	public class Surface : WorldLayer
	{
		private EMapBlockTypes[,] m_worldMap;

		public EMapBlockTypes[,] WorldMap
		{
			get
			{
				if (m_worldMap == null)
				{
					var wmg = new WorldMapGenerator(WorldMapSize);
					m_worldMap = wmg.Generate();
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
			get { return 150; }
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
			get { yield return ETerrains.BRICK_WALL; }
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);

			MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);



			for (var i = 3; i <= 8; i++)
			{
				block.Map[i, 3] = ETerrains.BRICK_WALL;
				block.Map[3, i] = ETerrains.BRICK_WALL;
				block.Map[i, 8] = ETerrains.BRICK_WALL;
				block.Map[8, i] = ETerrains.BRICK_WALL;
			}

			
			block.Map[6, 3] = ETerrains.WINDOW;
			block.Map[3, 6] = ETerrains.WINDOW;
			block.Map[6, 8] = ETerrains.GROUND;

			block.AddObject(ETiles.DOOR.GetThing(), new Point(6, 8));
			block.AddObject(new Sign(ETiles.SWORD, FColor.White, "'Оружейник'"), new Point(7, 8));

			block.AddLightSource(new Point(7, 9), new LightSource(15,new FColor(4f,1f,1f,0.5f)));

			{
				var cnt = rnd.Next(rnd.Next(70));
				for (var i = 0; i < cnt; ++i)
				{
					var x = rnd.Next(MapBlock.SIZE);
					var y = rnd.Next(MapBlock.SIZE);
					var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
					if (attr.IsPassable > 0)
					{
						block.Map[x, y] = ETerrains.MUSHROOM;
					}
				}
			}

			{
				var itmcnt = rnd.Next(rnd.Next(20));
				for (var i = 0; i < itmcnt; ++i)
				{
					var x = rnd.Next(MapBlock.SIZE);
					var y = rnd.Next(MapBlock.SIZE);

					var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
					if (attr.IsPassable > 0)
					{
						var point = new Point(x, y);
						var any = block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
						var thing =  World.Rnd.Next(2) == 0 ? ThingHelper.GetFaketThing(block) : ThingHelper.GetFaketItem(block.RandomSeed);

						if(thing.Is<Stair>() && (x==MapBlock.SIZE-1 || y==MapBlock.SIZE-1))
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

			{
				var x = rnd.Next(MapBlock.SIZE);
				var y = rnd.Next(MapBlock.SIZE);
				block.AddCreature(new Monster(this), new Point(x, y));
			}
			return block;
		}

		public override FColor Ambient
		{
			get
			{
				return new FColor(1f,1f,1f,0.5f).Multiply(0.1f); 
			}
		}
	}
}