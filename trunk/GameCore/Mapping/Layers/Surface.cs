using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;
using GameCore.Plants;
using RusLanguage;

namespace GameCore.Mapping.Layers
{
	public class Surface : WorldLayer
	{
		public const int WORLD_MAP_SIZE = 128;

		private static readonly List<string> m_maleNames;
		private static readonly List<string> m_femaleNames;
		private EMapBlockTypes[,] m_worldMap;
		private WorldMapGenerator2 m_worldMapGenerator;
		private static readonly List<ETiles> m_forestTiles = new List<ETiles>();

		static Surface()
		{
			m_maleNames = File.ReadAllText(@"Resources\malenicks.txt").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
			m_femaleNames = File.ReadAllText(@"Resources\femalenicks.txt").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

			for (var i = 0; i < 3; ++i) m_forestTiles.AddRange(ThingHelper.AllThings().Where(_furniture => _furniture.Is<Tree>()).Select(_tree => _tree.Tile));
			for (var i = 0; i < 2; ++i) m_forestTiles.AddRange(ThingHelper.AllThings().Where(_furniture => _furniture.Is<Shrub>()).Select(_tree => _tree.Tile));
			for (var i = 0; i < 1; ++i) m_forestTiles.AddRange(ThingHelper.AllThings().Where(_furniture => _furniture.Is<Mushrum>()).Select(_tree => _tree.Tile));
		}

		public Surface()
		{
		}

		public City City { get; private set; }

		public EMapBlockTypes[,] WorldMap
		{
			get
			{
				if (m_worldMap == null)
				{
					m_worldMapGenerator = new WorldMapGenerator2(WORLD_MAP_SIZE, World.Rnd);
					m_worldMap = m_worldMapGenerator.Generate();
					
					var cityBlockIds = m_worldMapGenerator.FindCityPlace((int)Math.Sqrt(WORLD_MAP_SIZE) / 2);
					foreach (var id in cityBlockIds)
					{
						m_worldMap[id.X,id.Y] = EMapBlockTypes.CITY;
					}
					City = new City(this, cityBlockIds);
				}
				return m_worldMap;
			}
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

		public EMapBlockTypes GetBlockType(Point _blockId) { return WorldMap[_blockId.X + WORLD_MAP_SIZE/2, _blockId.Y + WORLD_MAP_SIZE/2]; }

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
					case EMapBlockTypes.COAST:
					case EMapBlockTypes.LAKE_COAST:
					case EMapBlockTypes.ETERNAL_SNOW:
					case EMapBlockTypes.MOUNT:
					case EMapBlockTypes.CITY:
					case EMapBlockTypes.FOREST:
					case EMapBlockTypes.SHRUBS:
					case EMapBlockTypes.SWAMP:
					case EMapBlockTypes.GROUND:
						terrains.Add(blockTypes, DefaultEmptySpaces.ToArray());
						break;
					case EMapBlockTypes.SEA:
					case EMapBlockTypes.DEEP_SEA:
					case EMapBlockTypes.FRESH_WATER:
					case EMapBlockTypes.DEEP_FRESH_WATER:
						terrains.Add(blockTypes, new[] { ETerrains.FRESH_WATER, });
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
				switch (baseType)
				{
					case EMapBlockTypes.CITY:
					case EMapBlockTypes.GROUND:
						block.Map[point.X, point.Y] = terrains[baseType][rnd.Next(terrains[baseType].Length)];
						break;
					case EMapBlockTypes.SEA:
						{
							list.Add(baseType);
							list.Add(baseType);
							list.AddRange(from dPoint in Point.NearestDPoints select dPoint + xy into key where map.ContainsKey(key) select map[key]);

							var type = list.GroupBy(_types => _types).ToDictionary(_types => _types, _types => _types.Count()).OrderBy(_pair => _pair.Value).First().Key.Key;
							if (type != EMapBlockTypes.NONE)
							{
								block.Map[point.X, point.Y] = terrains[type][rnd.Next(terrains[type].Length)];
							}
						}
						break;
					case EMapBlockTypes.FOREST:
						block.Map[point.X, point.Y] = terrains[baseType][rnd.Next(terrains[baseType].Length)];
						{
							list.AddRange(from dPoint in Point.NearestDPoints select dPoint + xy into key where map.ContainsKey(key) select map[key]);

							var type = list.GroupBy(_types => _types).ToDictionary(_types => _types, _types => _types.Count()).OrderBy(_pair => _pair.Value + rnd.Next(5)).First().Key.Key;
							if (type != EMapBlockTypes.NONE && rnd.Next(3)==0)
							{
								block.AddObject(m_forestTiles[rnd.Next(m_forestTiles.Count)].GetThing(), point);
							}
						}
						break;
					case EMapBlockTypes.NONE:
						break;
					//default:
						//throw new ArgumentOutOfRangeException();
				}
			}


			if (baseType == EMapBlockTypes.CITY)
			{
				City.GenerateCityBlock(block, rnd, this);
			}

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

						if (thing.Is<Stair>())
						{
							if (x == BaseMapBlock.SIZE - 1 || y == BaseMapBlock.SIZE - 1)
							{
								continue;
							}
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
			return block;
		}
	}
}