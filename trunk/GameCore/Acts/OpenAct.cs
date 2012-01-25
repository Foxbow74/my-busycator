#region

using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Map;
using GameCore.Messages;
using GameCore.Objects;

#endregion

namespace GameCore.Acts
{
	public class OpenAct : Act
	{
		public OpenAct() : base(40)
		{
		}

		public override void Do(Creature _creature, World _world, bool _silence)
		{
			var mapCell = _world.Map.GetMapCell(_creature.Coords);

			var list = new List<MapCell>();
			var o = mapCell.Thing;
			if (o == null)
			{
				foreach (var cell in _creature.Coords.NearestPoints.Select(_point => _world.Map.GetMapCell(_point)))
				{
					if (cell.Thing == null) continue;
					if (cell.Thing is FakeItem)
					{
						cell.ResolveFakeItem();
					}
					if (cell.Thing is ICanbeOpened)
					{
						if (((ICanbeOpened) cell.Thing).IsClosed)
						{
							list.Add(cell);
						}
					}
				}
				if (list.Count == 0)
				{
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));
					return;
				}
				if (list.Count == 1)
				{
					((ICanbeOpened)list[0].Thing).Open(_creature, list[0]);
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