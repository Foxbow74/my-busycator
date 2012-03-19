using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts.Combat
{
	internal class ShootAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 30; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.T, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "выстрелить/метнуть"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

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

			var dPoint = GetParameter<Point>().FirstOrDefault();
			if (dPoint == null)
			{
				MessageManager.SendMessage(this, new AskShootTargerMessage(this, 10));
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
			if(dPoint==Point.Zero)
			{
				return EActResults.QUICK_FAIL;
			}
			new Missile(_creature.Layer, _creature.LiveCoords, 2, item, _creature.LiveCoords + dPoint);
			return EActResults.DONE;
		}
	}
}