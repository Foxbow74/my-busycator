using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
	public class WorldMapGenerator2
	{
		const float BORDER_WATER_PART = 0.3f;
		const float LAKE_PART = 0.2f;
		const float FOREST_PART = 0.2f;
		const float MOUNT_PART = 0.01f;
		const float LAKE_COAST_PART = 0.1f;
		const float SWAMP_PART = 0.1f;

		private readonly Random m_rnd;
		private readonly int m_size;
		private readonly int m_sqr;
		private readonly ushort[,] m_map;
		private readonly Rct m_rct;
		private readonly int m_imax;
		private int m_zones;
		private readonly bool[] m_forbidToUnite;
		private readonly int[] m_sizes;
		private readonly bool[,] m_neighbours;
		private readonly ushort[] m_united;
		private ushort[] m_allZones;
		private readonly Dictionary<EMapBlockTypes, MapTypeInfo> m_infos = new Dictionary<EMapBlockTypes, MapTypeInfo>();

		public WorldMapGenerator2(int _size, Random _rnd)
		{
			m_rnd = _rnd;
			m_size = _size;
			m_sqr = m_size * m_size;
			m_map = new ushort[m_size, m_size];
			m_rct = new Rct(0, 0, m_size, m_size);

			m_zones = m_imax = m_size * m_size / 2;
			m_forbidToUnite = new bool[m_imax + 1];
			m_sizes = new int[m_imax + 1];
			m_neighbours = new bool[m_imax + 1, m_imax + 1];
			m_united = new ushort[m_imax + 1];

			m_infos.Add(EMapBlockTypes.SEA, new MapTypeInfo(BORDER_WATER_PART));
			m_infos.Add(EMapBlockTypes.DEEP_SEA, new MapTypeInfo(0));
			m_infos.Add(EMapBlockTypes.COAST, new MapTypeInfo(0));
			m_infos.Add(EMapBlockTypes.LAKE_COAST, new MapTypeInfo(LAKE_COAST_PART));
			m_infos.Add(EMapBlockTypes.FRESH_WATER, new MapTypeInfo(LAKE_PART));
			m_infos.Add(EMapBlockTypes.DEEP_FRESH_WATER, new MapTypeInfo(0));
			m_infos.Add(EMapBlockTypes.ETERNAL_SNOW, new MapTypeInfo(MOUNT_PART));
			m_infos.Add(EMapBlockTypes.MOUNT, new MapTypeInfo(0));
			m_infos.Add(EMapBlockTypes.GROUND, new MapTypeInfo(0));
			m_infos.Add(EMapBlockTypes.FOREST, new MapTypeInfo(FOREST_PART));
			m_infos.Add(EMapBlockTypes.SWAMP, new MapTypeInfo(SWAMP_PART));
			m_infos.Add(EMapBlockTypes.SHRUBS, new MapTypeInfo(0));

		}

		public EMapBlockTypes[,] Generate()
		{
			return CreatePatchMap();
		}

		internal class MapTypeInfo
		{
			public float Part { get; private set; }
			public ushort Zone { get; set; }

			public MapTypeInfo(float _part)
			{
				Part = _part;
			}
		}

		public EMapBlockTypes[,] CreatePatchMap()
		{
			GenerateBaseMap();

			var result = new EMapBlockTypes[m_size, m_size];

			var zoneTypes = m_infos.Where(_valuePair => _valuePair.Value.Zone!=0).ToDictionary(_pair => _pair.Value.Zone, _pair => _pair.Key);
			for (var x = 0; x < m_size; ++x)
			{
				for (var y = 0; y < m_size; ++y)
				{
					var val = m_united[m_map[x, y]];
					EMapBlockTypes type;
					if (!zoneTypes.TryGetValue(val, out type))
					{
						type = EMapBlockTypes.GROUND;
					}
					result[x, y] = type;
				}
			}
			return result;
		}

		private void FillInnerPatches(IEnumerable<ushort> _list, EMapBlockTypes _type)
		{
			if(!_list.Any()) return;
			m_infos[_type].Zone = _list.First(_arg => !m_forbidToUnite[_arg]);
			foreach (var i in _list)
			{
				m_united[i] = m_infos[_type].Zone;
			}
		}

		private List<ushort> GetInnerPatches(EMapBlockTypes _type) {
			var zone = m_infos[_type].Zone;
			var list = new List<ushort>();
			foreach (var i in m_allZones)
			{
				var flag = true;
				if (m_united[i] != zone) continue;
				foreach (var j in m_allZones)
				{
					if(m_neighbours[i,j])
					{
						if (m_united[j] != zone)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(i);
				}
			}
			return list;
		}

		private void GenerateBaseMap() 
		{
			InitiallySeedAndGrowPatches();

			GenerateSea();
			
			#region формируем морской берег

			m_infos[EMapBlockTypes.COAST].Zone = Belt(m_infos[EMapBlockTypes.SEA].Zone);
			m_forbidToUnite[m_infos[EMapBlockTypes.COAST].Zone] = true;
			
			#endregion

			#region пускаем траву вдоль побережья

			m_infos[EMapBlockTypes.GROUND].Zone = Belt(m_infos[EMapBlockTypes.COAST].Zone);
			m_forbidToUnite[m_infos[EMapBlockTypes.GROUND].Zone] = true;
			
			#endregion

			var totalGround = m_sqr - m_sizes[m_infos[EMapBlockTypes.SEA].Zone] - m_sizes[m_infos[EMapBlockTypes.COAST].Zone] - m_sizes[m_infos[EMapBlockTypes.GROUND].Zone];

			#region добавляем снежные шапки и обносим их горами

			m_infos[EMapBlockTypes.ETERNAL_SNOW].Zone = SeedAndGrow(totalGround * m_infos[EMapBlockTypes.ETERNAL_SNOW].Part);

			m_forbidToUnite[m_infos[EMapBlockTypes.ETERNAL_SNOW].Zone] = true;

			m_infos[EMapBlockTypes.MOUNT].Zone = Belt(m_infos[EMapBlockTypes.ETERNAL_SNOW].Zone);
			m_forbidToUnite[m_infos[EMapBlockTypes.MOUNT].Zone] = true;

			#endregion

			#region леса

			m_infos[EMapBlockTypes.FOREST].Zone = SeedAndGrow(totalGround * m_infos[EMapBlockTypes.FOREST].Part);
			m_forbidToUnite[m_infos[EMapBlockTypes.FOREST].Zone] = true;

			#endregion

			#region озера

			m_infos[EMapBlockTypes.FRESH_WATER].Zone = SeedAndGrow(totalGround * m_infos[EMapBlockTypes.FRESH_WATER].Part);
			m_forbidToUnite[m_infos[EMapBlockTypes.FRESH_WATER].Zone] = true;

			#endregion

			#region берега озер

			m_infos[EMapBlockTypes.LAKE_COAST].Zone = Belt(m_infos[EMapBlockTypes.FRESH_WATER].Zone, 0, totalGround * m_infos[EMapBlockTypes.LAKE_COAST].Part);
			if (m_infos[EMapBlockTypes.LAKE_COAST].Zone != 0)
			{
				m_forbidToUnite[m_infos[EMapBlockTypes.LAKE_COAST].Zone] = true;
			}

			#endregion


			#region создание зоны глубокой пресной воды

			{
				var list = GetInnerPatches(EMapBlockTypes.FRESH_WATER);
				FillInnerPatches(list, EMapBlockTypes.DEEP_FRESH_WATER);
				if(m_infos[EMapBlockTypes.DEEP_FRESH_WATER].Zone!=0)
				{
					m_forbidToUnite[m_infos[EMapBlockTypes.DEEP_FRESH_WATER].Zone] = true;
				}
			}

			#endregion


			for (var i = 0; i < 1; i++)
			{
				UnitePatches();
			}

			#region болота

			m_infos[EMapBlockTypes.SWAMP].Zone = Belt(m_infos[EMapBlockTypes.FRESH_WATER].Zone, 0, totalGround * m_infos[EMapBlockTypes.SWAMP].Part);
			m_forbidToUnite[m_infos[EMapBlockTypes.SWAMP].Zone] = true;

			#endregion

			#region редколесье

			m_infos[EMapBlockTypes.SHRUBS].Zone = Belt(m_infos[EMapBlockTypes.FOREST].Zone, 0);

			#endregion

			#region создание зоны глубокой морской воды

			{
				var list = GetInnerPatches(EMapBlockTypes.SEA);
				list.Add(m_map[0, 0]);
				FillInnerPatches(list, EMapBlockTypes.DEEP_SEA);
			}

			#endregion
		}

		private void GenerateSea() 
		{
			Unite(m_rct.BorderPoints.Select(_point => m_map[_point.X, _point.Y]).Distinct());

			m_infos[EMapBlockTypes.SEA].Zone = m_united[m_map[0, 0]];
			m_forbidToUnite[m_infos[EMapBlockTypes.SEA].Zone] = true;

			#region топим пояса воды

			var process = true;
			var sum = 0;
			var part = m_infos[EMapBlockTypes.SEA].Part * m_sqr / 3 * 2;

			while (process)
			{
				var i = (ushort)(1 + m_rnd.Next(m_imax));

				while (m_united[i] == i && !m_forbidToUnite[i] && m_neighbours[i, m_infos[EMapBlockTypes.SEA].Zone])
				{
					sum += m_sizes[i];
					Unite(new[] { m_infos[EMapBlockTypes.SEA].Zone, i });
					process = sum < part;
					if (!process) break;

					foreach (var j in m_allZones)
					{
						if (!m_neighbours[i, j]) continue;
						i = j;
						break;
					}
				}
			}

			Belt(m_infos[EMapBlockTypes.SEA].Zone, m_infos[EMapBlockTypes.SEA].Zone, (m_size * m_size - m_sizes[m_infos[EMapBlockTypes.SEA].Zone]) * m_infos[EMapBlockTypes.SEA].Part);

			#endregion
		}

		private void UnitePatches()
		{
			foreach (var i in m_allZones)
			{
				if (m_united[i] != i || m_forbidToUnite[i]) { continue; }
				var j = (ushort)(m_rnd.Next(m_imax) + 1);
				{
					if (m_forbidToUnite[j] || m_united[j] != j || !m_neighbours[i, j]) continue;
					Unite(new [] { i, j });
				}
			}
		}

		private void InitiallySeedAndGrowPatches() 
		{
			var allZones = new List<ushort>();
			for (ushort i = 1; i <= m_imax; i++)
			{

				var xy = new Point(m_rnd.Next(m_size), m_rnd.Next(m_size));
				if (m_map[xy.X, xy.Y] == 0)
				{
					allZones.Add(i);
					m_sizes[i] = 1;
					m_united[i] = i;
					m_map[xy.X, xy.Y] = i;
				}
				else
				{
					i--;
				}
			}
			m_allZones = allZones.ToArray();

			var points = m_size*m_size - m_imax;
			var dpoints = Util.AllDirections.Select(_directions => _directions.GetDelta()).ToArray();
			while (points>0)
			{
				for (var x = 0; x < m_size; ++x)
				{
					for (var y = 0; y < m_size; ++y)
					{
						var xy = m_map[x, y];
						if(xy!=0)
						{
							var dpoint = dpoints[m_rnd.Next(4)];
							var x1 = x + dpoint.X;
							if (x1 < 0 || x1 == m_size) continue;
							var y1 = y + dpoint.Y;
							if (y1 < 0 || y1 == m_size) continue;
							var xy1 = m_map[x1, y1];
							if (xy1 == 0)
							{
								m_sizes[xy]++;
								m_map[x1, y1] = xy;
								points--;
							}
							else if(xy!=xy1)
							{
								if(xy1<xy)
								{
									var a = xy1;
									xy1 = xy;
									xy = a;
								}
								m_neighbours[xy, xy1] = true;
								m_neighbours[xy1, xy] = true;
							}
						}
					}
				}
			}
		}

		private ushort SeedAndGrow(float _part) 
		{
			var toUnite = new List<ushort>();
			var sum = 0;

			var len = m_forbidToUnite.Length;

			var tryes = 0;
			while (sum  < _part/10 && tryes<1000)
			{
				var zone = (ushort)m_rnd.Next(len);
				if (m_forbidToUnite[zone] || m_united[zone] != zone || toUnite.Contains(zone))
				{
					// если уже присоединена, или уже фиксированна
					tryes++;
					continue;
				}

				tryes = 0;
				
				toUnite.Add(zone);
				sum += m_sizes[zone];
			}

			if(toUnite.Count==0) return 0;

			var resultZone = toUnite[0];
			Unite(toUnite.Distinct());

			while (sum < _part)
			{
				Belt(resultZone, resultZone, _part - sum);
				if(sum<m_sizes[resultZone])
				{
					sum = m_sizes[resultZone];
				}
				else
				{
					break;
				}
			}
			return resultZone;
		}

		private ushort Belt(ushort _closeTo, ushort _connectTo = (ushort)0, float _notMoreThan = 0f)
		{
			var bltZones = new List<ushort>();
			if (_connectTo != 0) bltZones.Add(_connectTo);
			foreach (var i in m_allZones)
			{
				if (m_united[i] != i || m_forbidToUnite[i] || !m_neighbours[i, _closeTo]) continue;
				bltZones.Add(i);
				if(_notMoreThan>0)
				{
					_notMoreThan -= m_sizes[i];
					if(_notMoreThan<=0) break;
				}
			}
			Unite(bltZones.Distinct());
			return bltZones.FirstOrDefault();
		}

		private void Unite(IEnumerable<ushort> _zones)
		{
			ushort zoneI = 0;
			foreach (var zone in _zones)
			{
				if (zoneI == 0)
				{
					zoneI = zone;
				}
				else
				{
					m_united[zone] = zoneI;

					m_sizes[zoneI] += m_sizes[zone];
					m_sizes[zone] = 0;
					m_zones--;

					//присваиваем всех соседей
					foreach (var j in m_allZones)
					{
						if (m_neighbours[j, zone])
						{
							//neighbours[zone, j] = 0;
							//neighbours[j, zone] = 0;

							m_neighbours[zoneI, j] = true;
							m_neighbours[j, zoneI] = true;
						}
					}
				}
			}
		}

		public IEnumerable<Point> FindCityPlace(int _cityBlocks)
		{
			var result = new List<Point>();
			var lakeCoastZone = m_infos[EMapBlockTypes.LAKE_COAST].Zone;
			var groundZone = m_infos[EMapBlockTypes.GROUND].Zone;
			var lakeZone = m_infos[EMapBlockTypes.FRESH_WATER].Zone;
			var zones = new List<ushort>();
			zones.AddRange(m_allZones.Where(_i => m_united[_i]==groundZone && m_allZones.Any(_j => m_neighbours[_i,_j] && m_united[_j] == lakeZone)));
			if(!zones.Any())
			{
				zones.AddRange(m_allZones.Where(_i => m_united[_i] == lakeCoastZone && m_allZones.Any(_j => m_neighbours[_i, _j] && m_united[_j] == lakeZone)));
			}

			var center = new Point(m_size / 2, m_size / 2);
			while (true)
			{
				foreach (var point in center.GetSpiral(m_size))
				{
					var i = m_map[point.X, point.Y];
					if (zones.Contains(i))
					{
						for (var j = 0; j < 10; ++j)
						{
							result.Clear();
							var pnt = point;
							var toAdd = _cityBlocks;
							while (true)
							{
								result.Add(pnt);
								if (--toAdd == 0)
								{
									//result = m_rct.AllPoints.Where(_point => m_map[_point.X, _point.Y] == i).ToList();
									return result;
								}
								pnt += World.Rnd.GetRandomDirection().GetDelta();
								var unitedTo = m_united[m_map[pnt.X, pnt.Y]];
								if (unitedTo != groundZone && unitedTo != lakeCoastZone)
								{
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}
