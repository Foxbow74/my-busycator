using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Objects
{
	public static class ThingHelper
	{
		private static readonly Dictionary<Tuple<ETiles, FColor>, FakedThing> m_fakedThings = new Dictionary<Tuple<ETiles, FColor>, FakedThing>();
		private static readonly Dictionary<Tuple<ETiles, FColor>, FakedItem> m_fakedItems = new Dictionary<Tuple<ETiles, FColor>, FakedItem>();
		private static readonly Dictionary<Tuple<ETiles, FColor>, FakedMonster> m_fakedMonsters = new Dictionary<Tuple<ETiles, FColor>, FakedMonster>();

		//public static void RegisterThings()
		static ThingHelper()
		{
			foreach (var type in GetThingTypes())
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
				else if (_thing is FakedThing)
				{
					_thing = liveMapCell.ResolveFakeFurniture(_creature);
				}
			}
			return _thing.Name;
		}

		public static string GetName(this ThingDescriptor _thingDescriptor, Creature _creature)
		{
			var thing = _thingDescriptor.Thing;
			if (thing is IFaked)
			{
				thing = _thingDescriptor.ResolveThing(_creature);
			}
			return thing.Name;
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

		public static bool IsFurniture(this Thing _thing)
		{
			return _thing is FakedThing || _thing is Furniture.Furniture;
		}

		public static bool CanBeClosed(this Thing _thing, LiveMapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature);
			}
			return _thing is ICanbeClosed && ((ICanbeClosed) _thing).ELockType == ELockType.OPEN;
		}

		public static bool Is<T>(this Thing _thing)
		{
			return _thing != null && _thing.Is<T>();
		}

		public static bool IsFake(this Thing _thing)
		{
			return _thing is IFaked;
		}

		public static FakedThing GetThing(this ETiles _tile)
		{
			var key = new Tuple<ETiles, FColor>(_tile, FColor.Empty);
			FakedThing thing;
			return m_fakedThings.TryGetValue(key, out thing) ? thing : m_fakedThings.First(_pair => _pair.Key.Item1==_tile).Value;
		}

		public static FakedItem GetItem(this ETiles _tile)
		{
			return m_fakedItems[new Tuple<ETiles, FColor>(_tile, FColor.Empty)];
		}

		public static FakedMonster GetMonster(this ETiles _tile)
		{
			return m_fakedMonsters[new Tuple<ETiles, FColor>(_tile, FColor.Empty)];
		}

		private static void RegisterCreatureType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type, new object[] {null});
			FakedMonster value;
			var key = new Tuple<ETiles, FColor>(thing.Tile, thing.LerpColor);
			if (!m_fakedMonsters.TryGetValue(key, out value))
			{
				value = new FakedMonster(thing.Tile, thing.LerpColor);
				m_fakedMonsters.Add(key, value);
			}
			value.Add(_type);
		}

		private static void RegisterItemType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type);
			FakedItem value;
			var key = new Tuple<ETiles, FColor>(thing.Tile, thing.LerpColor);
			if (!m_fakedItems.TryGetValue(key, out value))
			{
				value = new FakedItem(thing.Tile, thing.LerpColor);
				m_fakedItems.Add(key, value);
			}
			value.Add(_type);
		}

		private static void RegisterThingType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type);
			var key = new Tuple<ETiles, FColor>(thing.Tile, thing.LerpColor);
			FakedThing value;
			if (!m_fakedThings.TryGetValue(key, out value))
			{
				value = new FakedThing(thing.Tile, thing.LerpColor);
				m_fakedThings.Add(key, value);
			}
			value.Add(_type);
		}

		private static IEnumerable<Type> GetThingTypes()
		{
			return from assembly in AppDomain.CurrentDomain.GetAssemblies()
			       from type in assembly.GetTypes()
			       where typeof (Thing).IsAssignableFrom(type) && !type.IsAbstract
			       select type;
		}

		public static Thing ResolveThing(Type _type, Creature _creature)
		{
			var thing = (Thing) Activator.CreateInstance(_type);
			thing.Resolve(_creature);
			return thing;
		}

		public static Thing GetFaketThing(MapBlock _block)
		{
			var keys = new List<Tuple<ETiles, FColor>>(m_fakedThings.Keys);
			var index = World.Rnd.Next(keys.Count);
			return m_fakedThings[keys[index]];
		}

		public static Thing GetFaketItem(int _blockRandomSeed)
		{
			var keys = new List<Tuple<ETiles, FColor>>(m_fakedItems.Keys);
			var index = World.Rnd.Next(keys.Count);
			return m_fakedItems[keys[index]];
		}

		public static Thing GetFaketCreature(MapBlock _block)
		{
			var keys = new List<Tuple<ETiles, FColor>>(m_fakedMonsters.Keys);
			return m_fakedMonsters[keys[World.Rnd.Next(keys.Count)]];
		}
	}
}