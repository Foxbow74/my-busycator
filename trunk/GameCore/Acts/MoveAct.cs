using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

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

			var mess = mapCell.TerrainAttribute.DisplayName;

			if (mapCell.IsPassable > 0)
			{
				_creature.Coords = pnt;

				if (!_silence)
				{
					var o = mapCell.Thing;
					if (o == null)
					{
						MessageManager.SendMessage(this, mess);
					}
					else
					{
						if (o is IFaked)
						{
							o = mapCell.ResolveFakeItem(World.TheWorld.Avatar);
						}
						MessageManager.SendMessage(this, mess + ", " + o.Name);
					}
				}

				if (isAvatar)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarMove);
				}
				return EActResults.DONE;
			}
			else
			{
				if (!_silence)
				{
					var o = mapCell.Thing;

					if(o.IsDoor(mapCell, _creature) && o.CanBeOpened(mapCell, _creature))
					{
						_creature.AddActToPool(new OpenAct(), pnt);
						return EActResults.DONE;
					}

					if (o != null)
					{
						mess = o.Name;
					}

					MessageManager.SendMessage(this, "неа, " + mess);
				}
				return EActResults.NOTHING_HAPPENS;
			}
		}
	}
}