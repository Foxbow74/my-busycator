#region

using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Mapping;
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

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var mapCell = Map.GetMapCell(_creature.Coords);

			var list = new List<MapCell>();
			var o = mapCell.Thing;
			if (o != null)
			{
				list.Add(mapCell);
			}
			foreach (var cell in _creature.Coords.NearestPoints.Select(Map.GetMapCell))
			{
				if (cell.Thing == null) continue;
				if (cell.Thing is FakeThing)
				{
					cell.ResolveFakeItem(World.TheWorld.Avatar);
				}
				if (cell.Thing is ICanbeOpened)
				{
					list.Add(cell);
				}
			}
			if (list.Count == 0)
			{
				if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "открыть что?"));
				return EActResults.NOTHING_HAPPENS;
			}
			if (list.Count == 1)
			{
				return ((ICanbeOpened) list[0].Thing).Open(_creature, list[0], _silence);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}