using System;
using Common.Messages;
using GameCore.Creatures;
using GameCore.Objects;

namespace GameCore.Acts
{
	public class TakeAct:Act
	{
		public TakeAct() : base(20)
		{
		}

		public override void Do(Creature _creatures, World _world, bool _silence)
		{
			var intelligent = (Intelligent)_creatures;

			var mapCell = _world.Map.GetMapCell(intelligent.Coords.X, intelligent.Coords.Y);
			var o = mapCell.Object;
			if(o==null)
			{
				if (!_silence) MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, "взять что?"));
			}
			else if(o is Container)
			{
				throw new NotImplementedException();
			}
			else if(o is Item)
			{
				mapCell.RemoveObjectFromBlock();
				intelligent.ObjectTaken((Item)o);

				if (intelligent is Avatar)
				{
					MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, o + " взят."));
				}
				else
				{
					MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, _creatures + " взял " + o));
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}