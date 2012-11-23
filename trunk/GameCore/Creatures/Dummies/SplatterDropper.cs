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

		private int m_tileindex;

		public SplatterDropper(WorldLayer _layer, Point _liveCoords, float _strength, FColor _color, Point _target)
			: base(_layer, 0)
		{
			m_tileindex = (int)(12*(1f - _strength));

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
				this[0, 0].AddSplatter(new Splatter(m_color, m_tileindex++));
			}

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
			if (passable < 0.1)
			{
				canMove = false;
			}

			//MessageManager.SendMessage(this, WorldMessage.JustRedraw);

			if(m_tileindex>11 || m_step>11)
			{
				canMove = false;
			}
			if (!canMove)
			{
				MessageManager.SendMessage(this, WorldMessage.Turn);
				return EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE;
			}
			AddActToPool(new MoveAct(), nextPoint);
			return EThinkingResult.NORMAL;
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			return null;
		}
	}
}