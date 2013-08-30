using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Mapping.Layers.SurfaceObjects;
using GameCore.Misc;
using GameCore.Plants;

namespace GameCore.Mapping.Layers
{
	/// <summary>
	/// Поверхность
	/// </summary>
    //public class Surface : WorldLayer
    //{

    //    private static List<string> m_maleNames;
    //    private static List<string> m_femaleNames;
    //    private readonly EMapBlockTypes[,] m_worldMap;

    //    static Surface()
    //    {
    //        LoadNicks();
    //    }

    //    /// <summary>
    //    /// Если размер мира 1*1 в зависимости от сида генерим тот или иной тестовый мир, иначе генерим по чсестному
    //    /// </summary>
    //    public Surface()
    //    {
    //        Ambient = new FColor(1f, 1f, 1f, 0.9f).Multiply(1.0f);

    //        switch (Constants.WORLD_MAP_SIZE)
    //        {
    //            case 1:
    //                var r1 = new TestSurfaceGenerator1x1(World.Rnd);
    //                m_worldMap = r1.Generate();
    //                City = new City(this, new Point(0,0));
    //                break;
    //            case 2:
    //                var r2 = new TestSurfaceGenerator2x2(World.Rnd);
    //                m_worldMap = r2.Generate();
    //                City = new City(this, new Point(0, 0));
    //                break;
    //            default:
    //                var worldMapGenerator = new WorldMapGenerator(Constants.WORLD_MAP_SIZE, World.Rnd);
    //                m_worldMap = worldMapGenerator.Generate();
    //                if (Constants.WORLD_MAP_SIZE > 31)
    //                {
    //                    var cityBlockIds = worldMapGenerator.FindCityPlace((int) Math.Sqrt(Constants.WORLD_MAP_SIZE)/2).ToArray();
    //                    foreach (var id in cityBlockIds)
    //                    {
    //                        m_worldMap[id.X, id.Y] = EMapBlockTypes.CITY;
    //                    }

    //                    City = new City(this, cityBlockIds.Select( _point => new Point(_point.X - Constants.WORLD_MAP_SIZE/2, _point.Y - Constants.WORLD_MAP_SIZE/2)).ToArray());
    //                }
    //                break;
    //        }
    //    }

    //    public City City { get; private set; }

    //    internal override IEnumerable<ETerrains> DefaultEmptySpaces { get { yield return ETerrains.GRASS; } }

    //    internal override IEnumerable<ETerrains> DefaultWalls { get { yield return ETerrains.RED_BRICK_WALL; } }

    //    private static void LoadNicks()
    //    {
    //        if (Constants.GAME_MODE == false || Constants.WORLD_MAP_SIZE < 4)
    //        {
    //            m_femaleNames = new List<string>();
    //            m_maleNames = new List<string>();
    //            for (int i = 1; i < 300; ++i)
    //            {
    //                m_femaleNames.Add("Female" + i);
    //                m_maleNames.Add("Male" + i);
    //            }
    //        }
    //        else
    //        {
    //            if (World.XResourceRoot.NickInfos.Count > 0)
    //            {
    //                var separator = new[] {','};
    //                foreach (var nicksInfo in World.XResourceRoot.NickInfos)
    //                {
    //                    switch (nicksInfo.Sex)
    //                    {
    //                        case ESex.MALE:
    //                            m_maleNames = nicksInfo.Nicks.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
    //                            break;
    //                        case ESex.FEMALE:
    //                            m_femaleNames = nicksInfo.Nicks.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
    //                            break;
    //                        default:
    //                            throw new ArgumentOutOfRangeException();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                throw new ApplicationException("База ресурсов не содержит информацию об именах.");
    //            }
    //        }
    //    }

    //    public Noun GetNextCitizenName(ESex _sex)
    //    {
    //        List<string> list;
    //        switch (_sex)
    //        {
    //            case ESex.MALE:
    //                list = m_maleNames;
    //                break;
    //            case ESex.FEMALE:
    //                list = m_femaleNames;
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException("_sex");
    //        }
    //        var result = list[World.Rnd.Next(list.Count)];
    //        if (list.Count > 1)
    //        {
    //            list.Remove(result);
    //        }
    //        return result.AsNoun(_sex, true);
    //    }

    //    //private static void PrepareNicks(string _filename, Random _rnd)
    //    //{
    //    //	var txt = File.ReadAllText(_filename);
    //    //	while (txt.IndexOf('(') > 0)
    //    //	{
    //    //		txt = txt.Replace(txt.Substring(txt.IndexOf('('), 3), "");
    //    //	}

    //    //	var names = txt.Split(new[] {",", " ", "\t", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
    //    //	var randomized = names.Distinct().OrderBy(_s => _rnd.Next());
    //    //	var result = String.Join(",", randomized);
    //    //	File.WriteAllText(_filename, result);
    //    //}

    //    public override EMapBlockTypes GetBlockType(Point _blockId)
    //    {
    //        var x = _blockId.X + Constants.WORLD_MAP_SIZE / 2;
    //        var y = _blockId.Y + Constants.WORLD_MAP_SIZE / 2;

    //        if (x < 0 || m_worldMap.GetLength(0) <= x || y < 0 || m_worldMap.GetLength(1) <= y)
    //        {
    //            return EMapBlockTypes.NONE;
    //        }
    //        return m_worldMap[x, y];
    //    }

    //    public override Point GetAvatarStartingBlockId()
    //    {
    //        return City == null ? new Point(-1, -1) : City.CityBlockIds[0];
    //    }

    //    protected override MapBlock GenerateBlock(Point _blockId)
    //    {
    //        var block = SurfaceBlockGenerator.GenerateBlock(_blockId, this);

    //        var rnd = new Random(block.RandomSeed);

    //        var baseType = GetBlockType(_blockId);
			
    //        if (baseType == EMapBlockTypes.CITY)
    //        {
    //            City.GenerateCityBlock(block, rnd, this);
    //        }

    //        var trees = EssenceHelper.GetAllThings<Tree>().ToArray();
    //        var shrubs = EssenceHelper.GetAllThings<Shrub>().ToArray();

    //        foreach (var point in new Rct(0, 0, Constants.MAP_BLOCK_SIZE, Constants.MAP_BLOCK_SIZE).AllPoints)
    //        {
    //            switch (block.Map[point.X, point.Y])
    //            {
    //                case ETerrains.FOREST:
    //                    switch (rnd.Next(10))
    //                    {
    //                        case 0:
    //                        case 1:
    //                            block.AddEssence(shrubs.RandomItem(rnd), point);
    //                            break;
    //                        case 2:
    //                        case 3:
    //                        case 4:
    //                            block.AddEssence(trees.RandomItem(rnd), point);
    //                            break;
    //                    }
    //                    break;
    //                case ETerrains.SHRUBS:
    //                    switch (rnd.Next(7))
    //                    {
    //                        case 0:
    //                        case 1:
    //                        case 2:
    //                        case 3:
    //                            block.AddEssence(shrubs.RandomItem(rnd), point);
    //                            break;
    //                        case 4:
    //                            block.AddEssence(trees.RandomItem(rnd), point);
    //                            break;
    //                    }
    //                    break;
    //            }
    //        }
    //        if(Constants.WORLD_MAP_SIZE==1)
    //        {
    //            TestSurfaceGenerator1x1.Fill(block, Constants.WORLD_SEED, baseType);
    //        }
    //        else
    //        {
    //            GenerateRandomItems(rnd, block);
    //        }
    //        return block;
    //    }

    //    private static void GenerateRandomItems(Random _rnd, MapBlock _block)
    //    {
    //        var itmcnt = 20 + _rnd.Next(_rnd.Next(20));
    //        for (var i = 0; i < itmcnt; ++i)
    //        {
    //            var x = _rnd.Next(Constants.MAP_BLOCK_SIZE);
    //            var y = _rnd.Next(Constants.MAP_BLOCK_SIZE);

    //            var attr = TerrainAttribute.GetAttribute(_block.Map[x, y]);
    //            if (attr.IsNotPassable) continue;


    //            var point = new Point(x, y);
    //            var thing = World.Rnd.Next(2) == 0 ? EssenceHelper.GetFakedThing(_rnd) : EssenceHelper.GetRandomFakedItem(_rnd);

    //            if (thing.Is<Stair>())
    //            {
    //                if (x == Constants.MAP_BLOCK_SIZE - 1 || y == Constants.MAP_BLOCK_SIZE - 1)
    //                {
    //                    continue;
    //                }
    //            }

    //            var any = _block.Objects.Where(_tuple => _tuple.Item2 == point).Select(_tuple => _tuple.Item1);
				
    //            if (thing is Item)
    //            {
    //                if (any.Any(_thing => !(_thing is Item)))
    //                {
    //                    continue;
    //                }
    //            }
    //            else if (any.Any())
    //            {
    //                continue;
    //            }

    //            _block.AddEssence(thing, point);
    //        }
    //    }
    //}
}