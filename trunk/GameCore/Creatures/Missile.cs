using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Combat;
using GameCore.Acts.Movement;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	class Missile : Creature, ISpecial
	{
		private readonly List<Point> m_path;
		private int m_step = 1;

		public Missile(Point _coords, int _speed, Item _ammo, Point _target)
			: base(_coords, _speed)
		{
			Ammo = _ammo;
			m_path = _coords.GetLineToPoints(_target).ToList();
		}

		public Item Ammo { get; private set; }

		public override ETiles Tile
		{
			get { return Ammo.Tile; }
		}

		public override string Name
		{
			get { return Ammo.Name; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		public override EThinkingResult Thinking()
		{
			var nextPoint = m_path[m_step];
			m_step++;

			var nextCell = Map.GetMapCell(nextPoint);
			if(nextCell.Creature!=null)
			{
				AddActToPool(new AtackAct(), nextPoint);
				return EThinkingResult.NORMAL;
			}
			var passable = nextCell.GetIsPassable(this);
			var canMove = m_step < m_path.Count;
			if (passable < 1)
			{
				nextCell = MapCell;
				canMove = false;
			}
			if(!canMove)
			{
				nextCell.AddObjectToBlock(Ammo);
				World.TheWorld.RemoveCreature(this);
				return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
			}
			AddActToPool(new MoveAct(), nextPoint - Coords);
			return EThinkingResult.NORMAL;
		}

		public override EActResults Atack(Creature _victim)
		{
			MessageManager.SendMessage(this, "попал!");
			World.TheWorld.RemoveCreature(this);
			return EActResults.SHOULD_BE_REMOVED_FROM_QUEUE;
		}
	}
}
