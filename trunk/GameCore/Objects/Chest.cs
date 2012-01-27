using System;
using System.Collections.Generic;
using GameCore.Acts;
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

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			if (LockType != LockType.OPEN)
			{
				if (!_silence) MessageManager.SendMessage(this, "готово");
				LockType = LockType.OPEN;
				return EActResults.DONE;
			}
			throw new NotImplementedException();
		}

		#endregion

		protected override IEnumerable<Item> GenerateItems(Creature _creature)
		{
			var cnt = World.Rnd.Next(_creature.GetLuckRandom);
			for (var i = 0; i < cnt; i++)
			{
				yield return (Item) MapBlockGenerator.GenerateFakeItem(_creature.MapBlock).Resolve(_creature);
			}
		}
	}
}