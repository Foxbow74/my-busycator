using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Combat;
using GameCore.Acts.Movement;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	internal class Missile : Creature, ISpecial
	{
		private readonly List<Point> m_path;
		private int m_step = 1;

		public Missile(WorldLayer _layer, Point _liveCoords, int _speed, Item _ammo, Point _target)
			: base(_layer, _speed)
		{
			Light = new LightSource(4, new FColor(2f, 1f, 0f, 0f));
			Ammo = _ammo;
			m_path = _liveCoords.GetLineToPoints(_target).ToList();
			LiveCoords = _liveCoords;
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

			var nextCell = World.TheWorld.LiveMap.GetCell(nextPoint);
			if (nextCell.Creature != null)
			{
				AddActToPool(new AtackAct(), nextPoint);
				return EThinkingResult.NORMAL;
			}
			var passable = nextCell.GetIsPassableBy(this);
			var canMove = m_step < m_path.Count;
			if (passable < 1)
			{
				nextCell = World.TheWorld.LiveMap.GetCell(LiveCoords);
				canMove = false;
			}
			if (!canMove)
			{
				nextCell.AddItem(Ammo);
				nextCell.LiveMapBlock.RemoveCreature(this);
				return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
			}
			AddActToPool(new MoveAct(), nextPoint - LiveCoords);
			return EThinkingResult.NORMAL;
		}

		public override EActResults Atack(Creature _victim)
		{
			MessageManager.SendMessage(this, "попал!");
			this[0,0].LiveMapBlock.RemoveCreature(this);
			return EActResults.DONE;
		}
	}
}