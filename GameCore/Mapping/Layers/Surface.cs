using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;
using Point = GameCore.Misc.Point;

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

		public int WorldMapSize
		{
			get { return 150; }
		}

		internal override IEnumerable<ETerrains> DefaultEmptyTerrains
		{
			get
			{
				yield return ETerrains.GRASS;
				yield return ETerrains.GROUND;
			}
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);

			MapBlockHelper.Clear(block, rnd, this);

			for (var i = 3; i <= 8; i++)
			{
				block.Map[i, 3] = ETerrains.BRICK_WALL;
				block.Map[3, i] = ETerrains.BRICK_WALL;
				block.Map[i, 8] = ETerrains.BRICK_WALL;
				block.Map[8, i] = ETerrains.BRICK_WALL;
			}

			//block.LightSources.Add(new Point(5, 2));
			block.Map[6, 3] = ETerrains.WINDOW;
			block.Map[3, 6] = ETerrains.WINDOW;
			block.Map[6, 8] = ETerrains.GROUND;
			block.AddObject(ETiles.DOOR.GetThing(), new Point(6, 8));

			//block.Map[7, 9] = ETerrains.BRICK_WALL;
			block.AddLightSource(new Point(7, 9), new LightSource(3, new FColor(Color.Blue)));

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
						var thig = World.Rnd.Next(2) == 0 ? ThingHelper.GetFaketThing(block) : ThingHelper.GetFaketItem(block.RandomSeed);
						if (thig is Item)
						{
							if (any.Any(_thing => !(thig is Item)))
							{
								continue;
							}
						}
						else if (any.Any())
						{
							continue;
						}
						block.AddObject(thig, point);
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
				return new FColor(Color.FromArgb(255, 255, 255, 40)); 
			}
		}
	}
}