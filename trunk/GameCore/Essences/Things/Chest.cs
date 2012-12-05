using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Acts.Items;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Essences.Things
{
	internal class Chest : Container, ICanbeOpened
	{
		public Chest(Material _material) : base(EALNouns.Chest, _material) { ELockType = ELockType.SIMPLE; }

        public override int TileIndex { get { return 7; } }

		#region ICanbeOpened Members

		public ELockType ELockType { get; private set; }

		public EActResults Open(Creature _creature, LiveMapCell _liveMapCell)
		{
			if (ELockType != ELockType.OPEN)
			{
				MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURE_OPENS_IT, _creature, this));
				ELockType = ELockType.OPEN;

				var collection = GetItems(_creature);
				if (collection.Any)
				{
					//обязать по любасу показать диалог выбора предметов
					_creature.AddActToPool(new TakeAct(), true, collection.Items, _liveMapCell.LiveCoords);
				}
				else
				{
					MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CONTAINER_IS_EMPTY, _creature, this));
				}

				return EActResults.DONE;
			}
			throw new NotImplementedException();
		}

		#endregion

		protected override IEnumerable<Item> GenerateItems(Creature _creature)
		{
			var cnt = _creature.GetLuckRandom*5.0;
			for (var i = 0; i < cnt; i++)
			{
				yield return EssenceHelper.GetRandomFakedItem(World.Rnd);
			}
		}
	}
}