using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Interact;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;
using GameCore.Objects.Furniture;

namespace GameCore.Acts.Movement
{
	public class MoveToAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 100; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.M, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "движение к точке"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.MOVEMENT; }
		}

		public override EActResults Do(Creature _creature)
		{
			var target = GetParameter<Point>().FirstOrDefault();

			if(target==null)
			{
				MessageManager.SendMessage(this, new AskDestinationMessage(this));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}

			var current = _creature[0,0].WorldCoords;
			if (target == current)
			{
				return EActResults.QUICK_FAIL;
			}
			var nextPoint = current.GetLineToPoints(target).ToArray()[1];
			var delta = nextPoint - current;
			var nextCell = _creature[delta];
			if(nextCell.GetIsPassableBy(_creature)>0)
			{
				_creature.AddActToPool(new MoveAct(), delta);
				return EActResults.ACT_REPLACED;
			}
			else if(nextCell.Furniture!=null && nextCell.Furniture.Is<Door>())
			{
				_creature.AddActToPool(new OpenAct(), delta);
				return EActResults.ACT_REPLACED;
			}
			return EActResults.FAIL;
		}
	}
}