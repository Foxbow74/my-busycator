using System;
using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Acts.Items;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects.Furniture
{
	public class Chest : Container, ICanbeOpened
	{
		public Chest()
		{
			LockType = LockType.SIMPLE;
		}

		public override float Opaque
		{
			get { return 0.5f; }
		}

		public override ETiles Tile
		{
			get { return ETiles.CHEST; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE; }
		}

		#region ICanbeOpened Members

		public LockType LockType { get; private set; }

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			if (LockType != LockType.OPEN)
			{
				if (!_silence) MessageManager.SendMessage(this, Name + " открыт.");
				LockType = LockType.OPEN;

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