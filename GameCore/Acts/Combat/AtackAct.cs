using System;
using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Acts.Combat
{
	internal class AtackAct : Act
	{
		protected override int TakeTicksOnSingleAction { get { return 80; } }

		public override IEnumerable<Tuple<ConsoleKey, EKeyModifiers>> ConsoleKeys { get { yield return new Tuple<ConsoleKey, EKeyModifiers>(ConsoleKey.A, EKeyModifiers.NONE); } }

		public override string Name { get { return "атаковать"; } }

		public override string HelpText { get { throw new NotImplementedException(); } }

		public override EActionCategory Category { get { return EActionCategory.COMBAT; } }

		public override EActResults Do(Creature _creature)
		{
			LiveMapCell liveMapCell;

			var find = FindCreature(_creature, (_crt, _cell) => _crt!=_creature, out liveMapCell);
			switch (find)
			{
				case EActResults.QUICK_FAIL:
					if (_creature.IsAvatar)
					{
						MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "направление атаки?"));
					}
					else
					{
						throw new ApplicationException("Только аватар может не указывать направление при атаке.");
					}
					return find;
				case EActResults.NONE:
					break;
				default:
					return find;

			}

			return World.TheWorld.BattleProcessor.Atack(_creature, liveMapCell.Creature);
		}
	}
}