#region

using System;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Objects;

#endregion

namespace GameCore.Acts
{
	public class TakeAct : Act
	{
		public TakeAct() : base(20)
		{
		}

		public override void Do(Creature _creature, bool _silence)
		{
			var intelligent = (Intelligent) _creature;

			var mapCell = Map.GetMapCell(intelligent.Coords);
			var thing = mapCell.Thing;

			if (thing == null || !(thing is Item))
			{
				if (!_silence)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "взять что?"));
				}
				return;
			}
			mapCell.RemoveObjectFromBlock();
			intelligent.ObjectTaken((Item) thing);

			if (intelligent is Avatar)
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, thing + " взят."));
			}
			else
			{
				MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " взял " + thing));
			}
		}
	}
}