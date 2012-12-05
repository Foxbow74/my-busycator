using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Acts.Interact;
using GameCore.Creatures;
using GameCore.Essences.Things;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Acts.Movement
{
	/// <summary>
	/// Point - точки смещения от начала маршрута
	/// int - максимальная длина пути
	/// </summary>
	public class MoveToAct : Act
	{
		public MoveToAct() { }

		public MoveToAct(Creature _creature, IEnumerable<Point> _pathFinderPath) { AddParameter(GetMoveToPath(_creature, _pathFinderPath)); }

		protected override int TakeTicksOnSingleAction { get { return 100; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.M, EKeyModifiers.NONE); } }

		public override EALConst Name { get { return EALConst.AN_MOVE_TO; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.MOVEMENT; } }

		public static IEnumerable<Point> GetMoveToPath(Creature _creature, IEnumerable<Point> _pathFinderPath)
		{
			var currPoint = _creature[0, 0].PathMapCoords;
			return _pathFinderPath.Select(_point => _point - currPoint);
		}

		public override EActResults Do(Creature _creature)
		{
			IEnumerable<Point> way;

			var max = int.MaxValue;
			TryGetParameter(out max);

			if (TryGetParameter(out way))
			{
				var result = EActResults.DONE;
				var pnt = way.First();
				var longWay = way.Count() > 1;
				foreach (var point in way)
				{
					var dpoint = point - pnt;
					if (dpoint != Point.Zero)
					{
						_creature.AddActToPool(new MoveAct(), dpoint, longWay);
						result = EActResults.ACT_REPLACED;
						max--;
						if (max <= 0)
						{
							break;
						}
					}
					pnt = point;
				}
				return result;
			}

			Point target;
			if (!TryGetParameter(out target))
			{
				MessageManager.SendMessage(this, new AskMessageNg(this, EAskMessageType.ASK_DESTINATION));
				return EActResults.NEED_ADDITIONAL_PARAMETERS;
			}

			var current = _creature[0, 0].WorldCoords;
			if (target == current)
			{
				return EActResults.QUICK_FAIL;
			}
			var nextPoint = current.GetLineToPoints(target).ToArray()[1];
			var delta = nextPoint - current;
			var nextCell = _creature[delta];
			if (nextCell.GetIsPassableBy(_creature) > 0)
			{
				_creature.AddActToPool(new MoveAct(), delta);
				return EActResults.ACT_REPLACED;
			}
			if (nextCell.Thing != null && nextCell.Thing.Is<ClosedDoor>())
			{
				_creature.AddActToPool(new OpenAct(), delta);
				return EActResults.ACT_REPLACED;
			}
			return EActResults.FAIL;
		}
	}
}