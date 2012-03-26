using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;
using RusLanguage;

namespace GameCore.Mapping.Layers
{
	public class Surface : WorldLayer
	{
		private static readonly List<string> m_maleNames;
		private static readonly List<string> m_femaleNames;
		private EMapBlockTypes[,] m_worldMap;
		private WorldMapGenerator m_worldMapGenerator;

		static Surface()
		{
			m_maleNames = File.ReadAllText(@"Resources\malenicks.txt").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
			m_femaleNames = File.ReadAllText(@"Resources\femalenicks.txt").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		public City City { get; private set; }

		public EMapBlockTypes[,] WorldMap
		{
			get
			{
				if (m_worldMap == null)
				{
					m_worldMapGenerator = new WorldMapGenerator(WorldMapSize, World.Rnd);
					m_worldMap = m_worldMapGenerator.Generate();

					City = new City(this, World.Rnd);
				}
				return m_worldMap;
			}
		}

		//public override float GetFogColorMultiplier(LiveMapCell _liveCell)
		//{
		//    return Math.Max(0.6f, 1f - _liveCell.TerrainAttribute.IsPassable);
		//}

		public int WorldMapSize
		{
			get { return 30; } // 150; }
		}

		internal override IEnumerable<ETerrains> DefaultEmptySpaces { get { yield return ETerrains.GRASS; } }

		internal override IEnumerable<ETerrains> DefaultWalls { get { yield return ETerrains.RED_BRICK_WALL; } }
		public override FColor Ambient { get { return new FColor(1f, 1f, 1f, 0.5f).Multiply(1f); } }

		public string GetNextCitizenName(ESex _sex)
		{
			List<string> list;
			switch (_sex)
			{
				case ESex.MALE:
					list = m_maleNames;
					break;
				case ESex.FEMALE:
					list = m_femaleNames;
					break;
				default:
					throw new ArgumentOutOfRangeException("_sex");
			}
			var result = list[World.Rnd.Next(list.Count)];
			list.Remove(result);
			return result;
		}

		private static void PrepareNicks(string _filename, Random _rnd)
		{
			var txt = File.ReadAllText(_filename);
			while (txt.IndexOf('(') > 0)
			{
				txt = txt.Replace(txt.Substring(txt.IndexOf('('), 3), "");
			}

			var names = txt.Split(new[] {",", " ", "\t", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			var randomized = names.Distinct().OrderBy(_s => _rnd.Next());
			var result = String.Join(",", randomized);
			File.WriteAllText(_filename, result);
			var check = File.ReadAllText(_filename);
		}

		public EMapBlockTypes GetBlockType(Point _blockId) { return WorldMap[_blockId.X + WorldMapSize/2, _blockId.Y + WorldMapSize/2]; }


		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = new MapBlock(_blockId);
			var rnd = new Random(block.RandomSeed);

			var centers = new Dictionary<Point, Point>();
			var dists = new Dictionary<Point, float>();
			foreach (var dPoint in Point.NearestDPoints)
			{
				centers[dPoint] = dPoint*BaseMapBlock.SIZE + BaseMapBlock.Rect.Center;
			}

			var terrains = new Dictionary<EMapBlockTypes, ETerrains[]>();
			foreach (EMapBlockTypes blockTypes in Enum.GetValues(typeof (EMapBlockTypes)))
			{
				switch (blockTypes)
				{
					case EMapBlockTypes.NONE:
						break;
					case EMapBlockTypes.CITY:
					case EMapBlockTypes.GROUND:
						terrains.Add(blockTypes, DefaultEmptySpaces.ToArray());
						break;
					case EMapBlockTypes.SEA:
						terrains.Add(blockTypes, new[] {ETerrains.WATER,});
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			var map = new Dictionary<Point, EMapBlockTypes>();

			foreach (var point in BaseMapBlock.Rect.AllPoints)
			{
				foreach (var center in centers)
				{
					dists[center.Key] = (center.Value - point).Lenght;
				}
				var dpoint = dists.OrderBy(_pair => _pair.Value + rnd.NextDouble()*16).First().Key;
				var type = GetBlockType(_blockId + dpoint);
				map[point] = type;
			}

			var list = new List<EMapBlockTypes>();
			var baseType = GetBlockType(_blockId);
			foreach (var point in BaseMapBlock.Rect.AllPoints)
			{
				list.Clear();
				var xy = point;
				list.Add(baseType);
				list.Add(baseType);
				list.AddRange(from dPoint in Point.NearestDPoints select dPoint + xy into key where map.ContainsKey(key) select map[key]);
				switch (baseType)
				{
					case EMapBlockTypes.CITY:
					case EMapBlockTypes.GROUND:
						block.Map[point.X, point.Y] = terrains[baseType][rnd.Next(terrains[baseType].Length)];
						break;
					case EMapBlockTypes.SEA:
						var type = list.GroupBy(_types => _types).ToDictionary(_types => _types, _types => _types.Count()).OrderBy(_pair => _pair.Value).First().Key.Key;
						if (type != EMapBlockTypes.NONE)
						{
							block.Map[point.X, point.Y] = terrains[type][rnd.Next(terrains[type].Length)];
						}
						break;
					case EMapBlockTypes.NONE:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}


			if (baseType == EMapBlockTypes.CITY)
			{
				City.GenerateCityBlock(block, rnd);
			}

			//var type = GetBlockType(_blockId);
			//switch (type)
			//{
			//    case EMapBlockTypes.GROUND:
			//        MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);
			//        break;
			//    case EMapBlockTypes.SEA:
			//        MapBlockHelper.Clear(block, rnd, this, new []{ETerrains.WATER, });
			//        break;
			//    case EMapBlockTypes.CITY:
			//        MapBlockHelper.Clear(block, rnd, this, DefaultEmptySpaces);
			//        City.GenerateCityBlock(block, rnd);
			//        break;
			//    case EMapBlockTypes.NONE:
			//        break;
			//    default:
			//        throw new ArgumentOutOfRangeException();
			//}


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
					var x = rnd.Next(BaseMapBlock.SIZE);
					var y = rnd.Next(BaseMapBlock.SIZE);

					var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
					if (attr.IsPassable > 0)
					{
						var point = new Point(x, y);
						var any = block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
						var thing = World.Rnd.Next(2) == 0 ? ThingHelper.GetFakedThing(block) : ThingHelper.GetFakedItem(block.RandomSeed);

						if (thing.Is<Stair>() && (x == BaseMapBlock.SIZE - 1 || y == BaseMapBlock.SIZE - 1))
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
	}
}