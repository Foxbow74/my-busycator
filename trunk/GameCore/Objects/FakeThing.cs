#region

using System;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

#endregion

namespace GameCore.Objects
{
	public class FakeThing : Thing, ICanbeOpened
	{
		public static readonly FakeThing Sword = new FakeThing(EThing.SWORD);
		public static readonly FakeThing Axe = new FakeThing(EThing.AXE);
		public static readonly FakeThing Chest = new FakeThing(EThing.CHEST);
		public static readonly FakeThing Door = new FakeThing(EThing.DOOR);

		public FakeThing(EThing _thing)
		{
			ThingType = _thing;
		}

		public EThing ThingType { get; private set; }

		public override ETiles Tile
		{
			get { return ThingType.Tile(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsClosed
		{
			get
			{
				switch (ThingType)
				{
					case EThing.CHEST:
					case EThing.DOOR:
						return true;
					default:
						return false;
				}
			}
		}

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			throw new ApplicationException();
		}

		#endregion

		public Thing Resolve(Creature _creature)
		{
			switch (ThingType)
			{
				case EThing.NONE:
					break;
				case EThing.SWORD:
					return new Sword();
				case EThing.AXE:
					return new Axe();
				case EThing.CHEST:
					return new Chest();
				case EThing.DOOR:
					return new Door();
				default:
					throw new ArgumentOutOfRangeException();
			}
			throw new NotImplementedException();
		}
	}
}