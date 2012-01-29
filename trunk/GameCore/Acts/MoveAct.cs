#region

using GameCore.Creatures;
using GameCore.Mapping;
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

		public override EActResults Do(Creature _creature, bool _silence)
		{
			var pnt = _creature.Coords + Delta;

			var isAvatar = _creature == World.TheWorld.Avatar;

			var mapCell = Map.GetMapCell(pnt);

			if (mapCell.IsPassable > 0)
			{
				_creature.Coords = pnt;

				var o = mapCell.Thing;
				if (o == null)
				{
					if (!_silence) MessageManager.SendMessage(this, mapCell.TerrainAttribute.DisplayName);
				}
				else
				{
					if (o is IFaked)
					{
						o = mapCell.ResolveFakeItem(World.TheWorld.Avatar);
					}
					if (!_silence) MessageManager.SendMessage(this, o.Name);
				}

				if (isAvatar)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarMove);
				}
				return EActResults.DONE;
			}
			else
			{
				if (!_silence) MessageManager.SendMessage(this, "неа, " + mapCell.TerrainAttribute.DisplayName);
				return EActResults.NOTHING_HAPPENS;
			}
		}
	}
}