#region

using System;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

#endregion

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		EActResults Open(Creature _creature, MapCell _mapCell, bool _silence);
	}

	internal class Door : Thing, ICanbeOpened
	{
		public override ETiles Tile
		{
			get { return ETiles.DOOR; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}