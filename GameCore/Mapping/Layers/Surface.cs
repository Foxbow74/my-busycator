using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furnitures;
using GameCore.Plants;
using GameCore.Storeable;
using RusLanguage;

namespace GameCore.Mapping.Layers
{
	/// <summary>
	/// Поверхность
	/// </summary>
	public class Surface : WorldLayer
	{

		private static readonly List<string> m_maleNames;
		private static readonly List<string> m_femaleNames;
		private readonly EMapBlockTypes[,] m_worldMap;

		static Surface()
		{
			var separator = new[] {','};

			if(World.XResourceRoot.NickInfos.Count>0)
			{
				foreach (var nicksInfo in World.XResourceRoot.NickInfos)
				{
					switch (nicksInfo.Sex)
					{
						case ESex.MALE:
							m_maleNames = nicksInfo.Nicks.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
							break;
						case ESex.FEMALE:
							m_femaleNames = nicksInfo.Nicks.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
			else
			{
                throw new ApplicationException("База ресурсов не содержит информацию об именах.");
                //if (File.Exists(Constants.RESOURCES_MALENICKS_TXT))
                //{
                //    m_maleNames =
                //        File.ReadAllText(Constants.RESOURCES_MALENICKS_TXT).Split(separator, StringSplitOptions.RemoveEmptyEntries).
                //            ToList();
                //}
                //else
                //{
                //    m_maleNames = new List<string> {"TestMale"};
                //}
                //if (File.Exists(Constants.RESOURCES_FEMALENICKS_TXT))
                //{
                //    m_femaleNames =
                //        File.ReadAllText(Constants.RESOURCES_FEMALENICKS_TXT).Split(separator, StringSplitOptions.RemoveEmptyEntries).
                //            ToList();
                //}
                //else
                //{
                //    m_femaleNames = new List<string> {"TestFemale"};
                //}

                //World.XResourceRoot.NickInfos.Add(new XNicksInfo() { Sex = ESex.FEMALE, Nicks = string.Join(",", m_femaleNames) });
                //World.XResourceRoot.NickInfos.Add(new XNicksInfo() { Sex = ESex.MALE, Nicks = string.Join(",", m_maleNames) });
                //World.SaveResources();
			}
		}

		/// <summary>
		/// Если размер мира 1*1 в зависимости от сида генерим тот или иной тестовый мир, иначе генерим по чсестному
		/// </summary>
		public Surface()
		{
			switch (Constants.WORLD_MAP_SIZE)
			{
				case 1:
					switch (Constants.WORLD_SEED)
					{
						case 1:
							var r = new TestSurfaceGenerator1x1(World.Rnd);
							m_worldMap = r.Generate();
							break;
						default:
							throw new ApplicationException("Йё!");
					}
					City = new City(this, new Point(0,0));
					break;
				default:
					var worldMapGenerator = new WorldMapGenerator(Constants.WORLD_MAP_SIZE, World.Rnd);
					m_worldMap = worldMapGenerator.Generate();
					var cityBlockIds = worldMapGenerator.FindCityPlace((int)Math.Sqrt(Constants.WORLD_MAP_SIZE) / 2);
					foreach (var id in cityBlockIds)
					{
						m_worldMap[id.X, id.Y] = EMapBlockTypes.CITY;
					}

					City = new City(this, cityBlockIds.Select(_point => new Point(_point.X-Constants.WORLD_MAP_SIZE/2,_point.Y-Constants.WORLD_MAP_SIZE/2)).ToArray());
					break;
			}
		}

		public City City { get; private set; }

		internal override IEnumerable<ETerrains> DefaultEmptySpaces { get { yield return ETerrains.GRASS; } }

		internal override IEnumerable<ETerrains> DefaultWalls { get { yield return ETerrains.RED_BRICK_WALL; } }
		
		public override FColor Ambient { get { return new FColor(1f, 1f, 1f, 0.5f).Multiply(0.8f); } }

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
			if (list.Count > 1)
			{
				list.Remove(result);
			}
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
		}

		public EMapBlockTypes GetBlockType(Point _blockId)
		{
			var x = _blockId.X + Constants.WORLD_MAP_SIZE / 2;
			var y = _blockId.Y + Constants.WORLD_MAP_SIZE / 2;

			if (x < 0 || m_worldMap.GetLength(0) <= x || y < 0 || m_worldMap.GetLength(1) <= y)
			{
				return EMapBlockTypes.NONE;
			}
			return m_worldMap[x, y];
		}

		protected override MapBlock GenerateBlock(Point _blockId)
		{
			var block = SurfaceBlockGenerator.GenerateBlock(_blockId, this);

			var rnd = new Random(block.RandomSeed);

			var baseType = GetBlockType(_blockId);
			
			if (baseType == EMapBlockTypes.CITY)
			{
				City.GenerateCityBlock(block, rnd, this);
			}

            var trees = ThingHelper.AllFakedFurniture().Where(_ff => _ff.Is<Tree>()).ToArray();
            var shrubs = ThingHelper.AllFakedFurniture().Where(_ff => _ff.Is<Shrub>()).ToArray();
            foreach (var point in new Rct(0, 0, Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE).AllPoints)
		    {
				switch (block.Map[point.X, point.Y])
			    {
					case ETerrains.FOREST:
						switch (rnd.Next(10))
						{
							case 0:
							case 1:
								block.AddObject(shrubs[rnd.Next(shrubs.Length)], point);
								break;
							case 2:
							case 3:
							case 4:
								block.AddObject(trees[rnd.Next(trees.Length)], point);
								break;
						}
						break;
					case ETerrains.SHRUBS:
						switch (rnd.Next(7))
						{
							case 0:
							case 1:
							case 2:
							case 3:
								block.AddObject(shrubs[rnd.Next(shrubs.Length)], point);
								break;
							case 4:
								block.AddObject(trees[rnd.Next(trees.Length)], point);
								break;
						}
						break;
			    }
		    }
			if(Constants.WORLD_MAP_SIZE==1)
			{
				TestSurfaceGenerator1x1.Fill(block, Constants.WORLD_SEED);
			}
			else
			{
				GenerateRandomItems(rnd, block);
			}
			return block;
		}

		private static void GenerateRandomItems(Random rnd, MapBlock block)
		{
			var itmcnt = 20 + rnd.Next(rnd.Next(20));
			for (var i = 0; i < itmcnt; ++i)
			{
				var x = rnd.Next(Constants.MAP_BLOCK_SIZE);
				var y = rnd.Next(Constants.MAP_BLOCK_SIZE);

				var attr = TerrainAttribute.GetAttribute(block.Map[x, y]);
				if (attr.IsPassable > 0)
				{
					var point = new Point(x, y);
					var any = block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
					var thing = World.Rnd.Next(2) == 0 ? ThingHelper.GetFakedThing(block) : ThingHelper.GetFakedItem(block.RandomSeed);

					if (thing.Is<Stair>())
					{
						if (x == Constants.MAP_BLOCK_SIZE - 1 || y == Constants.MAP_BLOCK_SIZE - 1)
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
	}
}