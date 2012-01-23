using System;
using System.Linq;
using System.Collections.Generic;
using Common.Messages;
using GameCore.Acts;
using GameCore.Creatures;
using Graphics;

namespace GameCore
{
	public class World
	{
		public long WorldTick { get; private set; }

		/// <summary>
		/// содержит список активных в данный момент существ
		/// </summary>
		private readonly List<Creature> m_activeCreatures = new List<Creature>();

		static World()
		{
			Rnd = new Random(1);
		}

		public World()
		{
			MessageManager.NewMessage += MessageManagerOnNewMessage;
			Map = new Map(this);

			Map[Point.Zero].Creatures.Add(new Monster(this, new Point(2, 2)));

			WorldTick = 0;

			Avatar = new Avatar(this);
			m_activeCreatures.Add(Avatar);
			MessageManager.SendMessage(this, new AvatarMovedMessage());
		}

		private void MessageManagerOnNewMessage(object _sender, Message _message)
		{
			if(_message is TurnMessage)
			{
				Act();
			}
			else if(_message is AvatarMovedMessage)
			{
				UpdateActiveCreatures();
			}
		}

		private void UpdateActiveCreatures()
		{
			m_activeCreatures.Clear();
			m_activeCreatures.AddRange(Map.GetBlocksNear(Avatar.Coords).SelectMany(_tuple => _tuple.Item2.Creatures));
		}

		public Map Map { get; private set; }

		public Avatar Avatar { get; private set; }

		public static Random Rnd{ get; private set; }

		/// <summary>
		/// Запускает мир до следующего хода игрока
		/// </summary>
		public void Act()
		{
			var act = Avatar.GetNextAct();
			ExecuteCreatureAct(Avatar, act);

			while (true)
			{
				var creature = m_activeCreatures[0];
				if (creature is Avatar)
				{
					break;
				}
				act = creature.GetNextAct();
				ExecuteCreatureAct(creature, act);
			}
		}

		private void ExecuteCreatureAct(Creature _creature, Act _act)
		{
			if (_act==null) throw new ArgumentNullException("_act");

			WorldTick = _creature.BusyTill;
			_creature.DoAct(_act);
			m_activeCreatures.Remove(_creature);
			var nextCreatureIndex = m_activeCreatures.FindIndex(_c => _c.BusyTill > _creature.BusyTill);
			if (nextCreatureIndex < 0) 
			{
				if (m_activeCreatures.Count > 0)
				{
					m_activeCreatures.Add(_creature);
				}
				else
				{
					m_activeCreatures.Insert(nextCreatureIndex + 1, _creature);
				}
			}
			else
			{
				m_activeCreatures.Insert(nextCreatureIndex, _creature);
			}
		}
	}
}