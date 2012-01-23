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
			var mapCell = _world.Map.GetMapCell(_creatures.Coords.X, _creatures.Coords.Y);
			var o = mapCell.Object;
			if(o==null)
			{
				if (!_silence) MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, "взять что?"));
			}
			else if(o is Container)
			{
				throw new NotImplementedException();
			}
			else
			{
				mapCell.RemoveObjectFromBlock();
				_creatures.ObjectTaken(o);

				if(_creatures is Avatar)
				{
					MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, o + " взят."));
				}
				else
				{
					MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, _creatures + " взял " + o));
				}
			}
		}
	}
}