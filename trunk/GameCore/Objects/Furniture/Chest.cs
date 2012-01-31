﻿using System;
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
		public override float Opaque
		{
			get
			{
				return 0.5f;
			}
		}

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

		public override void Resolve(Creature _creature)
		{
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE;}
		}

		#region ICanbeOpened Members

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

		protected override IEnumerable<Item> GenerateItems(Creature _creature)
		{
			var cnt = World.Rnd.Next(_creature.GetLuckRandom);
			for (var i = 0; i < cnt; i++)
			{
				yield return (Item)ThingHelper.GetFaketItem(_creature.MapBlock);
			}
		}
	}
}