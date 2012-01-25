#region

using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

#endregion

namespace GameCore.Acts
{
	public class MoveAct : Act
	{
		public MoveAct(Point _delta) : base(100)
		{
			Delta = _delta;
		}

		public Point Delta { get; set; }

		public override void Do(Creature _creature, World _world, bool _silence)
		{
			var pnt = _creature.Coords + Delta;

			var isAvatar = _creature is Avatar;

			var mapCell = _world.Map.GetMapCell(pnt);

			if (mapCell.IsPassable > 0)
			{
				_creature.Coords = pnt;

				var o = mapCell.Thing;
				if (o == null)
				{
					if (!_silence)
						MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.INFO, mapCell.TerrainAttribute.DisplayName));
				}
				else
				{
					if (o is FakeItem)
					{
						o = mapCell.ResolveFakeItem();
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
				if (!_silence)
					MessageManager.SendMessage(this,
					                           new SimpleTextMessage(EMessageType.INFO, "неа, " + mapCell.TerrainAttribute.DisplayName));
			}
		}
	}
}