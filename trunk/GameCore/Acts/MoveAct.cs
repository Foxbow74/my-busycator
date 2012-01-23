using Common.Messages;
using GameCore.Creatures;
using GameCore.Objects;
using Graphics;

namespace GameCore.Acts
{
	public class MoveAct:Act
	{
		public Point Delta { get; set; }

		public MoveAct(Point _delta):base(100)
		{
			Delta = _delta;
		}

		public override void Do(Creature _creatures, World _world, bool _silence)
		{
			var pnt = _creatures.Coords + Delta;

			var isAvatar = _creatures is Avatar;

			var mapCell = _world.Map.GetMapCell(pnt.X, pnt.Y);

			var attr = TerrainAttribute.GetAttribute(mapCell.Terrain);

			if (attr.IsPassable > 0)
			{
				_creatures.Coords = pnt;

				var o = mapCell.Object;
				if (o == null)
				{
					if (!_silence) MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, attr.DisplayName));
				}
				else
				{
					if (o is FakeItem)
					{
						o = mapCell.ResolveFakeItem((FakeItem)o);
					}
					if (!_silence) MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, o.Name));
				}

				if (isAvatar)
				{
					MessageManager.SendMessage(this, new AvatarMovedMessage());
				}
			}
			else
			{
				if (!_silence) MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, "неа, " + attr.DisplayName));
			}
		}
	}
}