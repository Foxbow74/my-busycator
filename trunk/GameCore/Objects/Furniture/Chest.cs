using System;
using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Acts.Items;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects.Furniture
{
	internal class Chest : Container, ICanbeOpened
	{
		public Chest()
		{
			ELockType = ELockType.SIMPLE;
		}

		public override ETiles Tile
		{
			get { return ETiles.CHEST; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}

		#region ICanbeOpened Members

		public ELockType ELockType { get; private set; }

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			if (ELockType != ELockType.OPEN)
			{
				if (!_silence) MessageManager.SendMessage(this, this.GetName(_creature) + " открыт.");
				ELockType = ELockType.OPEN;

				var collection = GetItems(_creature);
				if (collection.Any)
				{
					//обязать по любасу показать диалог выбора предметов
					_creature.AddActToPool(new TakeAct(), true, collection.Items, _mapCell.WorldCoords);
				}
				else
				{
					if (!_silence) MessageManager.SendMessage(this, "Увы, пусто.");
				}

				return EActResults.DONE;
			}
			throw new NotImplementedException();
		}

		#endregion

		public override void Resolve(Creature _creature)
		{
		}

		protected override IEnumerable<Item> GenerateItems(Creature _creature)
		{
			var cnt = _creature.GetLuckRandom*5.0;
			for (var i = 0; i < cnt; i++)
			{
				yield return (Item) ThingHelper.GetFaketItem(_creature.MapBlock);
			}
		}
	}
}