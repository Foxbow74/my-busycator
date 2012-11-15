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
		private readonly LightSource m_light;
		private readonly List<Point> m_path;
		private Point m_from;
		private int m_step = 1;

		private Point m_target;

		public Missile(WorldLayer _layer, Point _liveCoords, int _speed, Item _ammo, Point _target)
			: base(_layer, _speed)
		{
			m_target = _target;
			m_from = _liveCoords;
			m_light = new LightSource(4, new FColor(4f, 1f, 0.8f, 0.4f));
			Ammo = _ammo;
			m_path = _liveCoords.GetLineToPoints(_target).ToList();
			LiveCoords = _liveCoords;
		}

		public override Material Material { get { return Ammo.Material; } }

		public override ILightSource Light { get { return m_light; } }

		public Item Ammo { get; private set; }

		public override ETileset Tileset { get { return Ammo.Tileset; } }

		public override string Name { get { return Ammo.GetName(World.TheWorld.Avatar); } }

		public override void Resolve(Creature _creature) { }

		public override EThinkingResult Thinking()
		{
			var nextPoint = m_path[m_step] - m_path[m_step - 1];
			m_step++;

			var nextCell = this[nextPoint];
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
				LiveCoords = null;
				MessageManager.SendMessage(this, WorldMessage.Turn);
				return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
			}
			AddActToPool(new MoveAct(), nextPoint);
			return EThinkingResult.NORMAL;
		}

		public override EActResults Atack(Creature _victim)
		{
			MessageManager.SendMessage(this, "попал!");
			LiveCoords = null;
			return EActResults.DONE;
		}
	}
}