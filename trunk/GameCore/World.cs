using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Common.Messages;
using GameCore.Creatures;

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
			MessageManager.NewWorldMessage += MessageManagerOnNewMessage;
			Map = new Map(this);

			WorldTick = 0;

			Avatar = new Avatar(this);
			m_activeCreatures.Add(Avatar);
			MessageManager.SendMessage(this, WorldMessage.AvatarMove);
		}

		private void MessageManagerOnNewMessage(object _sender, WorldMessage _message)
		{
			switch (_message.Type)
			{
				case WorldMessage.EType.AVATAR_MOVE:
					UpdateActiveCreatures();
					break;
			}
		}

		private void UpdateActiveCreatures()
		{
			m_activeCreatures.Clear();
			m_activeCreatures.AddRange(Map.GetBlocksNear(Avatar.Coords).SelectMany(_tuple => _tuple.Item2.Creatures).OrderBy(_creature => _creature.BusyTill));
		}

		public Map Map { get; private set; }

		public Avatar Avatar { get; private set; }

		public static Random Rnd{ get; private set; }

		public void GameUpdated()
		{
			var done = new List<Creature>();
			while (true)
			{
				var creature = m_activeCreatures.FirstOrDefault();
				if (done.Contains(creature)) break;
				done.Add(creature);

				if (creature == null || creature.BusyTill > Avatar.BusyTill)
				{
					creature = Avatar;
				}

				var act = creature.GetNextAct();

				if (act==null)
				{
					break;
				}

				WorldTick = WorldTick < creature.BusyTill ? creature.BusyTill : WorldTick;
				creature.DoAct(act);

				if(creature==Avatar) continue;

				m_activeCreatures.Remove(creature);
				var nextCreatureIndex = m_activeCreatures.FindIndex(_c => _c.BusyTill > creature.BusyTill);
				if (nextCreatureIndex < 0)
				{
					if (m_activeCreatures.Count > 0)
					{
						m_activeCreatures.Add(creature);
					}
					else
					{
						m_activeCreatures.Insert(nextCreatureIndex + 1, creature);
					}
				}
				else
				{
					m_activeCreatures.Insert(nextCreatureIndex, creature);
				}
			}
			MessageManager.SendMessage(this, WorldMessage.Turn);
		}
	}
}