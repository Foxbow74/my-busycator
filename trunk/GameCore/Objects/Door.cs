#region

using System;
using GameCore.Creatures;
using GameCore.Map;

#endregion

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		bool IsClosed { get; }
		void Open(Creature _creature, MapCell _mapCell);
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

		public bool IsClosed
		{
			get { throw new NotImplementedException(); }
		}

		public void Open(Creature _creature, MapCell _mapCell)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}