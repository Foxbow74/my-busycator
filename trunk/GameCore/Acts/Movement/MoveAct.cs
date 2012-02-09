using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Interact;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Acts.Movement
{
	public class MoveAct : Act
	{
		protected override int TakeTicksOnSingleAction
		{
			get { return 100; }
		}

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys
		{
			get { return KeyTranslator.MoveKeys.Select(_key => new Tuple<ConsoleKey, EKeyModifiers>(_key, EKeyModifiers.NONE)); }
		}

		public override string Name
		{
			get { return "Движение (стороны света)"; }
		}

		public override string HelpText
		{
			get { throw new NotImplementedException(); }
		}

		public override string HelpKeys
		{
			get { return "стрелки"; }
		}

		public override EActionCategory Category
		{
			get { return EActionCategory.MOVEMENT; }
		}

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var delta = GetParameter<Point>().FirstOrDefault();

			if (delta == null)
			{
				var key = GetParameter<ConsoleKey>().Single();
				delta = KeyTranslator.GetDirection(key);
			}

			var pnt = _creature.Coords + delta;
			var mapCell = _creature.Layer.GetMapCell(pnt);

			if (mapCell.GetIsPassableBy(_creature) > 0)
			{
				var mess = mapCell.TerrainAttribute.DisplayName;

				if (!_silence)
				{
					var furniture = mapCell.Furniture;
					if (furniture != null)
					{
						mess += ", " + furniture.GetName(_creature, pnt);
					}
					var items = mapCell.Items.ToArray();
					if (items.Length > 0)
					{
						if (items.Length == 1)
						{
							mess += ", " + items[0].GetName(_creature, pnt);
						}
						else
						{
							mess += ", вещи";
						}
					}
					MessageManager.SendMessage(this, mess);
				}
				_creature.Coords = pnt;
				return EActResults.DONE;
			}
			else
			{
				var mess = String.Empty;
				if (!_silence)
				{
					var creature = mapCell.Creature;
					if (creature != null)
					{
						mess = creature.GetName(_creature);
					}
					else
					{
						var furniture = mapCell.Furniture;
						if (furniture != null)
						{
							if (furniture.IsDoor(mapCell, _creature) && furniture.IsClosed(mapCell, _creature))
							{
								_creature.AddActToPool(new OpenAct(), pnt);
								return EActResults.DONE;
							}
							else
							{
								mess = furniture.GetName(_creature);
							}
						}
						else
						{
							mess = mapCell.TerrainAttribute.DisplayName;
						}
					}
					MessageManager.SendMessage(this, "неа, " + mess);
				}
				return EActResults.NOTHING_HAPPENS;
			}
		}
	}
}