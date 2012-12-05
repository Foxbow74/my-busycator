using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Acts.Combat;
using GameCore.Acts.Interact;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Things;
using GameCore.Messages;
using GameCore.Misc;
using GameCore;

namespace GameCore.Acts.Movement
{
	public class MoveAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 100; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { return KeyTranslator.MoveKeys.Select(_key => new Tuple<ConsoleKey, EKeyModifiers>(_key, EKeyModifiers.NONE)); } }

		public override EALConst Name { get { return EALConst.AN_MOVE; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override string HelpKeys { get { return EALConst.AN_MOVE_ARROWS.GetString(); } }

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
				var mess = "";// cell.TerrainAttribute.DisplayName;

				if (_creature.IsAvatar)
				{
					var thing = cell.Thing;
					if (thing != null)
					{
						mess += ", " + EALSentence.NONE.GetString(thing.GetName(_creature, cell));
					}
					var items = cell.Items.ToArray();
					if (items.Length > 0)
					{
						if (items.Length == 1)
						{
							mess += ", " + EALSentence.NONE.GetString(items[0].GetName(_creature));
						}
						else
						{
							mess += ", вещи";
						}
					}
					MessageManager.SendMessage(this, mess);
				}
				World.TheWorld.CreatureManager.MoveCreatureOnDelta(_creature, delta);
				return EActResults.DONE;
			}
			else
			{
				XMessage mess;
				if (_creature.IsAvatar)
				{
					var creature = cell.Creature;
					if (creature != null)
					{
						bool isMoveToAct;
						if(!TryGetParameter(out isMoveToAct) || !isMoveToAct)
						{
							_creature.AddActToPool(new AtackAct(), delta);
							return EActResults.ACT_REPLACED;

							////Если это не перемещение на дальнее расстояние
							//return World.TheWorld.BattleProcessor.Atack(_creature, creature);
						}
						mess = new XMessage(EALTurnMessage.CELL_IS_OCCUPIED_BY, _creature, creature);
					}
					else
					{
						var thing = cell.Thing;
						if (thing != null && thing.Is<ClosedDoor>() && thing.IsLockedFor(cell, _creature))
						{
							_creature.InsertActToPool(new OpenAct(), delta);
							return EActResults.ACT_REPLACED;
						}
						mess = new XMessage(EALTurnMessage.CREATURE_NOW_STAY_ON, _creature, cell.Terrain.AsNoun());
					}
					MessageManager.SendXMessage(this, mess);
				}
				return EActResults.QUICK_FAIL;
			}
		}
	}
}