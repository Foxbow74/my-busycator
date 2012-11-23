using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Interact;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Acts.Movement
{
	public class MoveAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 100; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { return KeyTranslator.MoveKeys.Select(_key => new Tuple<ConsoleKey, EKeyModifiers>(_key, EKeyModifiers.NONE)); } }

		public override string Name { get { return "движение (стороны света)"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override string HelpKeys { get { return "стрелки"; } }

		public override EActionCategory Category { get { return EActionCategory.MOVEMENT; } }

		public override EActResults Do(Creature _creature)
		{
			Point delta;
			if (!TryGetParameter(out delta))
			{
				delta = KeyTranslator.GetDirection(GetFirstParameter<ConsoleKey>());
			}

			if (delta.QLenght > 1)
			{
				throw new ApplicationException("Элементарное перемещение длиннее чем 1");
			}

			if (delta == Point.Zero)
			{
				_creature.AddActToPool(new WaitAct());
				return EActResults.ACT_REPLACED;
			}

			var cell = _creature[delta];

			if (cell.GetIsPassableBy(_creature) > 0)
			{
				var mess = cell.TerrainAttribute.DisplayName;

				if (_creature.IsAvatar)
				{
					var thing = cell.Thing;
					if (thing != null)
					{
						mess += ", " + thing.GetName(_creature, cell);
					}
					var items = cell.Items.ToArray();
					if (items.Length > 0)
					{
						if (items.Length == 1)
						{
							mess += ", " + items[0].GetName(_creature, cell);
						}
						else
						{
							mess += ", вещи";
						}
					}
					MessageManager.SendMessage(this, mess);
				}
				_creature.LiveCoords += delta;
				return EActResults.DONE;
			}
			else
			{
				var mess = String.Empty;
				if (_creature.IsAvatar)
				{
					var creature = cell.Creature;
					if (creature != null)
					{
						bool isMoveToAct;
						if(!TryGetParameter(out isMoveToAct) || !isMoveToAct)
						{
							//Если это не перемещение на дальнее расстояние
							return World.TheWorld.BattleProcessor.Atack(_creature, creature);
						}
						mess = creature.GetName(_creature);
					}
					else
					{
						var thing = cell.Thing;
						if (thing != null && thing.Is<ClosedDoor>() && thing.IsLockedFor(cell, _creature))
						{
							_creature.InsertActToPool(new OpenAct(), delta);
							return EActResults.ACT_REPLACED;
						}
						mess = cell.TerrainAttribute.DisplayName;
					}
					MessageManager.SendMessage(this, "неа, тут " + mess);
				}
				return EActResults.QUICK_FAIL;
			}
		}
	}
}