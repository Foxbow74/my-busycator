using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Objects
{
	public static class ThingHelper
	{
		private static readonly Dictionary<ETiles, FakedThing> m_fakedThings = new Dictionary<ETiles, FakedThing>();
		private static readonly Dictionary<ETiles, FakedItem> m_fakedItems = new Dictionary<ETiles, FakedItem>();
		private static readonly Dictionary<ETiles, FakedMonster> m_fakedMonsters = new Dictionary<ETiles, FakedMonster>();

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
		/// <param name = "_coords">Координаты не всегда совпадают с координатами существа</param>
		/// <returns></returns>
		public static string GetName(this Thing _thing, Creature _creature, Point _coords = null)
		{
			if (_coords == null)
			{
				_coords = _creature.Coords;
			}
			if (_thing is IFaked)
			{
				var mapCell = _creature.Layer.GetMapCell(_coords);
				if (_thing is Item)
				{
					_thing = mapCell.ResolveFakeItem(World.TheWorld.Avatar, (FakedItem) _thing);
				}
				else if (_thing is Furniture.Furniture)
				{
					_thing = mapCell.ResolveFakeFurniture(World.TheWorld.Avatar, (FakedThing) _thing);
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

		public static bool IsClosed(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature, (FakedThing) _thing);
			}
			return _thing is ICanbeOpened && ((ICanbeOpened) _thing).ELockType != ELockType.OPEN;
		}

		public static bool IsFurniture(this Thing _thing)
		{
			return _thing is FakedThing || _thing is Furniture.Furniture;
			;
		}

		public static bool CanBeClosed(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature, (FakedThing) _thing);
			}
			return _thing is ICanbeClosed && ((ICanbeClosed) _thing).ELockType == ELockType.OPEN;
		}

		public static bool IsDoor(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature, (FakedThing) _thing);
			}
			return _thing is Door || _thing is OpenDoor;
		}

		public static bool IsChest(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeFurniture(_creature, (FakedThing) _thing);
			}
			return _thing is Chest;
		}

		public static bool IsFake(this Thing _thing)
		{
			return _thing is IFaked;
		}

		public static FakedThing GetThing(this ETiles _tile)
		{
			return m_fakedThings[_tile];
		}

		public static FakedItem GetItem(this ETiles _tile)
		{
			return m_fakedItems[_tile];
		}

		public static FakedMonster GetMonster(this ETiles _tile)
		{
			return m_fakedMonsters[_tile];
		}

		private static void RegisterCreatureType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type, new object[] {null, Point.Zero});
			FakedMonster value;
			if (!m_fakedMonsters.TryGetValue(thing.Tile, out value))
			{
				value = new FakedMonster(thing.Tile);
				m_fakedMonsters.Add(thing.Tile, value);
			}
			value.Add(_type);
		}

		private static void RegisterItemType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type);
			FakedItem value;
			if (!m_fakedItems.TryGetValue(thing.Tile, out value))
			{
				value = new FakedItem(thing.Tile);
				m_fakedItems.Add(thing.Tile, value);
			}
			value.Add(_type);
			//Debug.WriteLine(thing.Name);
		}

		private static void RegisterThingType(Type _type)
		{
			var thing = (Thing) Activator.CreateInstance(_type);
			FakedThing value;
			if (!m_fakedThings.TryGetValue(thing.Tile, out value))
			{
				value = new FakedThing(thing.Tile);
				m_fakedThings.Add(thing.Tile, value);
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
			var keys = new List<ETiles>(m_fakedThings.Keys);
			var index = World.Rnd.Next(keys.Count);
			return m_fakedThings[keys[index]];
		}

		public static Thing GetFaketItem(MapBlock _block)
		{
			var keys = new List<ETiles>(m_fakedItems.Keys);
			var index = World.Rnd.Next(keys.Count);
			return m_fakedItems[keys[index]];
		}

		public static Thing GetFaketCreature(MapBlock _block)
		{
			var keys = new List<ETiles>(m_fakedMonsters.Keys);
			return m_fakedMonsters[keys[World.Rnd.Next(keys.Count)]];
		}
	}
}