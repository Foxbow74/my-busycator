using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

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

			var mapCell = _world.Map.GetMapCell(pnt);

			if (mapCell.IsPassable > 0)
			{
				_creatures.Coords = pnt;

				var o = mapCell.Object;
				if (o == null)
				{
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, mapCell.TerrainAttribute.DisplayName));
				}
				else
				{
					if (o is FakeItem)
					{
						o = mapCell.ResolveFakeItem((FakeItem)o);
					}
					if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, o.Name));
				}

				if (isAvatar)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarMove);
				}
			}
			else
			{
				if (!_silence) MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, "неа, " + mapCell.TerrainAttribute.DisplayName));
			}
		}
	}
}