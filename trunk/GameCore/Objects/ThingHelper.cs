using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Objects
{
	public static class ThingHelper
	{
		public static bool IsItem(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			return _thing is Item;
		}

		public static bool IsClosed(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeItem(_creature);
			}
			return _thing is ICanbeOpened && ((ICanbeOpened)_thing).LockType != LockType.OPEN;
		}

		public static bool IsDoor(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeItem(_creature);
			}
			return _thing is Door;
		}

		public static bool IsChest(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				_thing = _cell.ResolveFakeItem(_creature);
			}
			return _thing is Chest;
		}

		public static bool IsFake(this Thing _thing)
		{
			return _thing is IFaked;
		}

		private static readonly Dictionary<ETiles, FakedThing> m_fakedThings = new Dictionary<ETiles, FakedThing>();
		private static readonly Dictionary<ETiles, FakedItem> m_fakedItems = new Dictionary<ETiles, FakedItem>();
		private static readonly Dictionary<ETiles, FakedMonster> m_fakedMonsters = new Dictionary<ETiles, FakedMonster>();

		public static void RegisterThings()
		{
			foreach (var type in GetThingTtypes())
			{
				if (typeof(IFaked).IsAssignableFrom(type)) continue;
				if (typeof(BackPack).IsAssignableFrom(type)) continue;

				if (typeof(Creature).IsAssignableFrom(type))
				{
					RegisterCreatureType(type);
				}
				else if (typeof(Item).IsAssignableFrom(type))
				{
					RegisterItemType(type);
				}
				else 
				{
					RegisterThingType(type);
				}
			}
		}

		private static void RegisterCreatureType(Type _type)
		{
			if(typeof(Avatar).IsAssignableFrom(_type))
			{
				return;
			}
			var thing = (Thing)Activator.CreateInstance(_type, new object[]{Point.Zero});
			FakedMonster value;
			if (!m_fakedMonsters.TryGetValue(thing.Tile, out value))
			{
				value = new FakedMonster(thing.Tile);
				m_fakedMonsters.Add(thing.Tile, value);
			}
			value.Add(_type);
			Debug.WriteLine(thing.Name);
		}

		private static void RegisterItemType(Type _type)
		{
			var thing = (Thing)Activator.CreateInstance(_type);
			FakedItem value;
			if (!m_fakedItems.TryGetValue(thing.Tile, out value))
			{
				value = new FakedItem(thing.Tile);
				m_fakedItems.Add(thing.Tile, value);
			}
			value.Add(_type);
			Debug.WriteLine(thing.Name);
		}

		private static void RegisterThingType(Type _type)
		{
			var thing = (Thing)Activator.CreateInstance(_type);
			FakedThing value;
			if (!m_fakedThings.TryGetValue(thing.Tile, out value))
			{
				value = new FakedThing(thing.Tile);
				m_fakedThings.Add(thing.Tile, value);
			}
			value.Add(_type);
			Debug.WriteLine(thing.Name);
		}

		private static IEnumerable<Type> GetThingTtypes()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if (typeof(Thing).IsAssignableFrom(type) && !type.IsAbstract && !type.IsNotPublic)
					{
						yield return type;
					}
				}
			}
		}

		public static Thing ResolveThing(Type _type, Creature _creature)
		{
			var thing = (Thing)Activator.CreateInstance(_type);
			thing.Resolve(_creature);
			Debug.WriteLine(thing.Name);
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