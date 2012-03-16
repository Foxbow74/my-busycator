using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts.Movement
{
	public class MoveToAct : Act, ISpecial
	{
		protected override int TakeTicksOnSingleAction
		{
			get { throw new NotImplementedException(); }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { throw new NotImplementedException(); }
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
			var target = GetParameter<Point>().First();
			var current = _creature[0,0].WorldCoords;
			if (target == current)
			{
				return EActResults.NOTHING_HAPPENS;
			}
			var nextPoint = current.GetLineToPoints(target).ToArray()[1];
			var delta = nextPoint - current;
			if(_creature[delta].GetIsPassableBy(_creature)>0)
			{
				_creature.AddActToPool(new MoveAct(), delta);
				_creature.AddActToPool(this);
			}
			return EActResults.NOTHING_HAPPENS;
		}
	}
}