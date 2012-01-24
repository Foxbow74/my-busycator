using System;
using System.Collections.Generic;
using System.Linq;
using Common.Messages;
using GameCore.Creatures;
using GameCore.Objects;
using Graphics;
using MS.Internal.Xml.XPath;
using Object = GameCore.Objects.Object;

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

	public class OpenAct:Act
	{
		public OpenAct(int _takeTicks) : base(_takeTicks)
		{
		}

		public override void Do(Creature _creatures, World _world, bool _silence)
		{
			var mapCell = _world.Map.GetMapCell(_creatures.Coords);

			var list=new List<Object>();
			var o = mapCell.Object;
			if (o == null)
			{
				list.AddRange(_creatures.Coords.NearestPoints.Select(_point => _world.Map.GetMapCell(_point).Object).Where(_o => _o!=null));
				if(list.Count==0)
				{
					if (!_silence) MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, "открыть что?"));	
					return;
				}
				if(list.Count==1)
				{
					
				}
				else 
				foreach (var cell in )
				{
					if(cell.Object!=null);
				}
				

				
			}
			else
			{
				
			}
		}
	}
}