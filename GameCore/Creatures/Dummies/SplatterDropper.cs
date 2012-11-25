using System.Collections.Generic;
using System.Linq;
using GameCore.Acts.Movement;
using GameCore.Essences.Weapons;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Creatures.Dummies
{
	public class SplatterDropper : AbstractDummyCreature
	{
		private readonly FColor m_color;
		private readonly List<Point> m_path;
		private int m_step = 1;

		private int m_strenght;

		public SplatterDropper(WorldLayer _layer, LiveMapCell _from, int _strength, FColor _color, LiveMapCell _to): base(_layer, 0)
		{
			m_strenght = _strength;
			m_color = _color;

			var d = _to.PathMapCoords - _from.PathMapCoords;
			LiveCoords = _from.LiveCoords;
			m_path = LiveCoords.GetLineToPoints(LiveCoords + d * 10).ToList();
		}

		public override ETileset Tileset { get { return ETileset.NONE; } }

		public override string Name { get { return ""; } }

		public override void Resolve(Creature _creature) { }

		public override EThinkingResult Thinking()
		{
			if (m_step > 1)
			{
				m_strenght -= this[0, 0].AddSplatter(m_strenght, m_color);
			}

			var canMove = m_step < (m_path.Count - 1);
			Point nextPoint = null;
			if(canMove && m_strenght>0)
			{
				nextPoint = m_path[m_step] - m_path[m_step - 1];
				m_step++;

				var nextCell = this[nextPoint];
				if (nextCell.Creature != null)
				{
					m_strenght -= this[nextPoint].AddSplatter(m_strenght, m_color);
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