using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Objects
{
	public abstract class Thing
	{
		public abstract ETiles Tile { get; }
		public abstract string Name { get; }

		public override string ToString()
		{
			return Name;
		}
	}

	public static class ThingHelper
	{
		public static bool IsItem(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				var fth = (FakeThing) _thing;
				switch (fth.ThingType)
				{
					case EThing.DOOR:
						return false;
				}
				return true;
			}
			return _thing is Item;
		}

		public static bool IsClosed(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				var fth = (FakeThing)_thing;
				switch (fth.ThingType)
				{
					case EThing.CHEST:
					case EThing.DOOR:
						_thing = _cell.ResolveFakeItem(_creature);
						break;
					default:
						return false;
				}
			}
			return _thing is ICanbeOpened && ((ICanbeOpened)_thing).LockType != LockType.OPEN;
		}

		public static bool IsDoor(this Thing _thing, MapCell _cell, Creature _creature)
		{
			if (_thing == null) return false;
			if (_thing.IsFake())
			{
				var fth = (FakeThing)_thing;
				return fth.ThingType == EThing.DOOR;
			}
			return _thing is Door;
		}

		public static bool IsFake(this Thing _thing)
		{
			return _thing is FakeThing;
		}
	}
}