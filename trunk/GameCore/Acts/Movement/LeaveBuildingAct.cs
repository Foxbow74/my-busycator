using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Objects;

namespace GameCore.Acts.Movement
{
	internal class LeaveBuildingAct : Act, ISpecial
	{
		protected override int TakeTicksOnSingleAction { get { return 100; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { throw new NotImplementedException(); } }

		public override string Name { get { return "покинуть помещение"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.MOVEMENT; } }

		public override EActResults Do(Creature _creature)
		{
			var building = _creature[0, 0].InBuilding;
			if (building != null)
			{
				var coords = _creature[0, 0].WorldCoords;
				var p = building.OutDoorWorldCoords[World.Rnd.Next(building.OutDoorWorldCoords.Length)];
				if (_creature[p - coords].GetIsPassableBy(_creature) > 0)
				{
					_creature.AddActToPool(new MoveToAct(), p);
					return EActResults.ACT_REPLACED;
				}
				return EActResults.QUICK_FAIL;
			}
			return EActResults.DONE;
		}
	}
}