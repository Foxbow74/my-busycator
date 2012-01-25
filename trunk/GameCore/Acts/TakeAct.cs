using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Messages;
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

			var mapCell = _world.Map.GetMapCell(intelligent.Coords);
			var o = mapCell.Object;
			if(o==null)
			{
				if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "взять что?"));
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
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, o + " взят."));
				}
				else
				{
					MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, _creatures + " взял " + o));
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}

	public class OpenAct:Act
	{
		public OpenAct() : base(40)
		{
		}

		public override void Do(Creature _creatures, World _world, bool _silence)
		{
			var mapCell = _world.Map.GetMapCell(_creatures.Coords);

			var list=new List<MapCell>();
			var o = mapCell.Object;
			if (o == null)
			{
				list.AddRange(_creatures.Coords.NearestPoints.Select(_point => _world.Map.GetMapCell(_point)).Where(_cell => _cell.Object != null && _cell.Object is ICanbeOpened && ((ICanbeOpened)_cell.Object).IsClosed));
				if(list.Count==0)
				{
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));	
					return;
				}
				if(list.Count==1)
				{
					Open(list[0]);
				}
				else
				{
					//foreach (var cell in)
					//{
					//    if (cell.Object != null) ;
					//}
				}
			}
			else
			{
				
			}
		}

		private void Open(MapCell _mapCell)
		{
			throw new NotImplementedException();
		}
	}
}