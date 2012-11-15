using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Objects
{
	public static class ThingHelper
	{
		private static readonly Dictionary<Tuple<ETiles, Material>, FakedFurniture> m_fakedThings = new Dictionary<Tuple<ETiles, Material>, FakedFurniture>();
		private static readonly Dictionary<Tuple<ETiles, Material>, FakedItem> m_fakedItems = new Dictionary<Tuple<ETiles, Material>, FakedItem>();
		private static readonly Dictionary<Tuple<ETiles, Material>, FakedCreature> m_fakedMonsters = new Dictionary<Tuple<ETiles, Material>, FakedCreature>();
		private static readonly List<Material> m_materials = new List<Material>();

		static ThingHelper()
		{
			foreach (var type in Util.GetAllTypesOf<Material>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;

				var material = (Material) Activator.CreateInstance(type);
				m_materials.Add(material);
			}

			foreach (var type in Util.GetAllTypesOf<Thing>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;

				if (typeof (Creature).IsAssignableFrom(type))
				{
					RegisterCreatureType(type);
				}
				else if (typeof (Item).IsAssignableFrom(type))
				{
					RegisterItemType(type);
				}
				else
				{
					RegisterThingType(type);
				}
			}
		}

		public static IEnumerable<FakedFurniture> AllThings()
		{
			var result = m_fakedThings.Select(_pair => _pair.Value).Distinct().ToArray();
			return result;
		}

		public static IEnumerable<FakedItem> AllItems()
		{
			var result = m_fakedItems.Select(_pair => _pair.Value).Distinct().ToArray();
			return result;
		}

		//public static void RegisterThings()

		/// <summary>
		/// 	Возвращает имя объекта
		/// 	Если надо - резолвит
		/// </summary>
		/// <param name = "_thing"></param>
		/// <param name = "_creature"></param>
		/// <param name = "_cell">Координаты не всегда совпадают с координатами существа</param>
		/// <returns></returns>
		public static string GetName(this Thing _thing, Creature _creature, LiveMapCell _cell = null)
		{
			if (_cell == null)
			{
				_cell = _creature[Point.Zero];
			}
			if (_thing is IFaked)
			{
				var liveMapCell = _cell;
				if (_thing is FakedItem)
				{
					_thing = liveMapCell.ResolveFakeItem(World.TheWorld.Avatar, (FakedItem) _thing);
				}
				else if (_thing is FakedFurniture)
				{
					_thing = liveMapCell.ResolveFakeFurniture(_creature);
				}
			}
			return _thing.GetFullName();
		}

		public static string GetName(this ThingDescriptor _thingDescriptor, Creature _creature)
		{
			var thing = _thingDescriptor.Thing;
			if (thing is IFaked)
			{
				thing = _thingDescriptor.ResolveThing(_creature);
			}
			return thing.GetFullName();
		}

		public static bool IsClosed(this Thing _thing, LiveMapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature);
			}
			return _thing is ICanbeOpened && ((ICanbeOpened) _thing).ELockType != ELockType.OPEN;
		}

		public static bool CanBeClosed(this Thing _thing, LiveMapCell _cell, Creature _creature)
		{
			if (!(_thing is FurnitureThing)) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature);
			}
			return _thing is ICanbeClosed && ((ICanbeClosed) _thing).ELockType == ELockType.OPEN;
		}

		public static bool Is<T>(this Thing _thing) { return _thing != null && _thing.Is<T>(); }

		public static bool IsFake(this Thing _thing) { return _thing is IFaked; }

		public static FakedFurniture GetThing(this ETiles _tile, Material _material = null)
		{
			if (_material == null)
			{
				var arr = m_fakedThings.Where(_pair => _pair.Key.Item1 == _tile).ToArray();
				return arr.RandomItem(World.Rnd).Value;
			}
			var key = new Tuple<ETiles, Material>(_tile, _material);
			FakedFurniture furniture;
			return m_fakedThings.TryGetValue(key, out furniture) ? furniture : m_fakedThings.First(_pair => _pair.Key.Item1 == _tile).Value;
		}

		public static FakedItem GetItem(this ETiles _tile, Material _material = null)
		{
			if (_material == null)
			{
				var arr = m_fakedItems.Where(_pair => _pair.Key.Item1 == _tile).ToArray();
				return arr.RandomItem(World.Rnd).Value;
			}
			return m_fakedItems[new Tuple<ETiles, Material>(_tile, _material)];
		}

		public static FakedCreature GetMonster(this ETiles _tile, Material _material = null)
		{
			if (_material == null)
			{
				var arr = m_fakedMonsters.Where(_pair => _pair.Key.Item1 == _tile).ToArray();
				return arr.RandomItem(World.Rnd).Value;
			}
			return m_fakedMonsters[new Tuple<ETiles, Material>(_tile, _material)];
		}

		private static void RegisterCreatureType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type, new object[] {null});
			FakedCreature value;
			var key = new Tuple<ETiles, Material>(thing.Tileset, GetMaterial<FlashMaterial>());
			if (!m_fakedMonsters.TryGetValue(key, out value))
			{
				value = new FakedCreature(thing.Tileset);
				m_fakedMonsters.Add(key, value);
			}
			value.Add(_type);
		}

		public static IEnumerable<EMaterial> GetAllowedMaterials(EMaterial _materials)
		{
			var allowedMaterials = (from EMaterial value in Enum.GetValues(typeof (EMaterial)) where _materials.HasFlag(value) select value).ToArray();
			return allowedMaterials;
		}

		private static void RegisterItemType(Type _type)
		{
			var athing = (Thing) Activator.CreateInstance(_type, new object[] {null});
			foreach (var mtp in GetAllowedMaterials(athing.AllowedMaterials))
			{
				var mtpLocal = mtp;
				foreach (var material in m_materials.Where(_material => _material.MaterialType == mtpLocal))
				{
					var thing = (Thing) Activator.CreateInstance(_type, new object[] {material});

					FakedItem value;
					var key = new Tuple<ETiles, Material>(thing.Tileset, material);
					if (!m_fakedItems.TryGetValue(key, out value))
					{
						value = new FakedItem(thing.Tileset, material);
						m_fakedItems.Add(key, value);
					}
					value.Add(_type);
				}
			}
		}

		private static void RegisterThingType(Type _type)
		{
			var athing = (Thing) Activator.CreateInstance(_type, new object[] {null});
			foreach (var mtp in GetAllowedMaterials(athing.AllowedMaterials))
			{
				var mtpLocal = mtp;
				foreach (var material in m_materials.Where(_material => _material.MaterialType == mtpLocal))
				{
					var thing = (Thing) Activator.CreateInstance(_type, new object[] {material});
					var key = new Tuple<ETiles, Material>(thing.Tileset, material);
					FakedFurniture value;
					if (!m_fakedThings.TryGetValue(key, out value))
					{
						value = new FakedFurniture(thing);
						m_fakedThings.Add(key, value);
					}
				}
			}
		}

		public static Thing ResolveThing(Type _type, Material _material, Creature _creature)
		{
			var thing = (Thing) Activator.CreateInstance(_type, new object[] {_material});
			thing.Resolve(_creature);
			return thing;
		}

		public static Thing GetFakedThing(MapBlock _block)
		{
			var keys = new List<Tuple<ETiles, Material>>(m_fakedThings.Keys);
			var index = World.Rnd.Next(keys.Count);
			return m_fakedThings[keys[index]];
		}

		public static FakedItem GetFakedItem(int _blockRandomSeed)
		{
			var keys = new List<Tuple<ETiles, Material>>(m_fakedItems.Keys);
			var index = World.Rnd.Next(keys.Count);
			return m_fakedItems[keys[index]];
		}

		public static FakedCreature GetFakedCreature(MapBlock _block)
		{
			var keys = new List<Tuple<ETiles, Material>>(m_fakedMonsters.Keys);
			return m_fakedMonsters[keys[World.Rnd.Next(keys.Count)]];
		}

		public static TMaterial GetMaterial<TMaterial>() where TMaterial : Material { return (TMaterial) m_materials.First(_material => _material is TMaterial); }
	}
}