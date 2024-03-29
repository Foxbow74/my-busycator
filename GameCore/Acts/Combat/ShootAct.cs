﻿using System;
using System.Collections.Generic;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Acts.Combat
{
	internal class ShootAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 200; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.T, EKeyModifiers.NONE); } }

		public override EALConst Name { get { return EALConst.AN_SHOOT; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.COMBAT; } }

		public override EActResults Do(Creature _creature)
		{
			var intelligent = (Intelligent) _creature;
			var item = intelligent[EEquipmentPlaces.MISSILES];
			if (item == null)
			{
				if (_creature.IsAvatar)
				{
					MessageManager.SendMessage(this, "Экипируйте снаряд для метания");
					return EActResults.FAIL;
				}
				else
				{
					throw new ApplicationException("Нечем стрелять");
				}
			}

			Point dPoint;
			if (!TryGetParameter(out dPoint))
			{
				MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.ASK_SHOOT_TARGET, 10));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}

			if (item is StackOfItems)
			{
				var stack = (StackOfItems) item;
				item = stack.GetOne();
				if (stack.Count == 0)
				{
					intelligent.TakeOff(EEquipmentPlaces.MISSILES);
					intelligent.RemoveFromBackpack(stack);
				}
			}
			else
			{
				intelligent.TakeOff(EEquipmentPlaces.MISSILES);
				intelligent.RemoveFromBackpack(item);
			}
			if (dPoint == Point.Zero)
			{
				return EActResults.QUICK_FAIL;
			}
			new Missile(_creature.GeoInfo.Layer, _creature[0, 0], 2, item, _creature[dPoint.X, dPoint.Y]);
			return EActResults.DONE;
		}
	}
}