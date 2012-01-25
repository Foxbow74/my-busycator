#region

using System;
using GameCore.Creatures;
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

		public override void Do(Creature _creature, World _world, bool _silence)
		{
			var intelligent = (Intelligent) _creature;

			var mapCell = _world.Map.GetMapCell(intelligent.Coords);
			var o = mapCell.Thing;
			if (o == null)
			{
				if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "взять что?"));
			}
			else if (o is Container)
			{
				throw new NotImplementedException();
			}
			else if (o is Item)
			{
				mapCell.RemoveObjectFromBlock();
				intelligent.ObjectTaken((Item) o);

				if (intelligent is Avatar)
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, o + " взят."));
				}
				else
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creature + " взял " + o));
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}