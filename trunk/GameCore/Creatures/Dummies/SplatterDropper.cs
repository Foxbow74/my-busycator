using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Combat;
using GameCore.Acts.Movement;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Creatures.Dummies
{
	public class SplatterDropper : AbstractDummyCreature
	{
		private readonly FColor m_color;
		private readonly List<Point> m_path;
		private int m_step = 1;

		private int m_strenght;

		public SplatterDropper(WorldLayer _layer, Point _liveCoords, int _strength, FColor _color, Point _target)
			: base(_layer, 0)
		{
			m_strenght = _strength;
			m_color = _color;
			var d = _target - _liveCoords;
			m_path = _liveCoords.GetLineToPoints(_liveCoords + d*10).ToList();
			LiveCoords = _liveCoords;
		}

		public override ETileset Tileset { get { return ETileset.NONE; } }

		public override string Name { get { return ""; } }

		public override void Resolve(Creature _creature) { }

		public override EThinkingResult Thinking()
		{
			if (m_step > 1)
			{
				int tileIndex = 0;
				if (m_strenght >= Splatter.COUNT)
				{
					tileIndex = World.Rnd.Next(Splatter.COUNT);
				}
				else
				{
					tileIndex = Splatter.COUNT - World.Rnd.Next(m_strenght) - 1;
				}
				m_strenght -= Splatter.COUNT - tileIndex;

				this[0, 0].AddSplatter(new Splatter(m_color, tileIndex));
			}

			var canMove = m_step < (m_path.Count - 1);
			Point nextPoint = null;
			if(canMove)
			{
				nextPoint = m_path[m_step] - m_path[m_step - 1];
				m_step++;

				var nextCell = this[nextPoint];
				if (nextCell.Creature != null)
				{
					this[nextPoint].AddSplatter(new Splatter(m_color, Splatter.COUNT/2 + World.Rnd.Next(Splatter.COUNT / 2)));
					return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
				}
				var passable = nextCell.GetIsPassableBy(this);
				canMove = m_step < m_path.Count;
				if (passable < 0.1)
				{
					canMove = false;
				}
			}
			if (m_strenght <= 0 || !canMove)
			{
				return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
			}
			else
			{
				AddActToPool(new MoveAct(), nextPoint);
				return EThinkingResult.NORMAL;
			}
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			return null;
		}
	}
}