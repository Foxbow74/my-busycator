using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Movement;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Acts.Combat
{
	class AtackAct:Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 20; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.A, EKeyModifiers.NONE); }
		}

		public override string Name
		{
			get { return "атаковать"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.COMBAT; }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var coords = GetParameter<Point>().FirstOrDefault();
			if(coords==null)
			{
				throw new NotImplementedException();
			}
			var victim = _creature.MapBlock.Creatures.FirstOrDefault(_cr => _cr.Coords == coords);
			if(victim==null)
			{
				_creature.AddActToPool(new MoveAct(), coords-_creature.Coords);
				return EActResults.NOTHING_HAPPENS;
			}
			return _creature.Atack(victim);
		}
	}
}
