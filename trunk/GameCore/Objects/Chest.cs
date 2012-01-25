using System;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects
{
	public class Chest : Container, ICanbeOpened
	{

		public Chest()
		{
			LockType = LockType.SIMPLE;
		}

		public LockType LockType { get; private set; }

		public override ETiles Tile
		{
			get { return ETiles.CHEST; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}

		#region ICanbeOpened Members

		public bool IsClosed
		{
			get
			{
				switch (LockType)
				{
					case LockType.OPEN:
						return false;
					case LockType.SIMPLE:
						return true;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void Open(Creature _creature, MapCell _mapCell)
		{
			LockType = LockType.OPEN;
			var cnt = World.Rnd.Next(_creature.GetLuckRandom);
			for (var i = 0; i < cnt; i++)
			{
				Items.Add((Item)MapBlockGenerator.GenerateFakeItem(_creature.MapBlock).Resolve(_creature));
			}
			MessageManager.SendMessage(this, new SelectItemsMessage(Items));
		}

		#endregion
	}
}