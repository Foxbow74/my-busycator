using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore
{
	public class World
	{
		/// <summary>
		/// 	содержит список активных в данный момент существ
		/// </summary>
		private readonly List<Creature> m_activeCreatures = new List<Creature>();

		static World()
		{
			Rnd = new Random(1);
		}

		public World()
		{
			MessageManager.NewWorldMessage += MessageManagerOnNewMessage;
			Map = new Map();

			WorldTick = 0;

			Avatar = new Avatar();
			m_activeCreatures.Add(Avatar);
			MessageManager.SendMessage(this, WorldMessage.AvatarMove);
		}

		public static World TheWorld { get; private set; }

		public long WorldTick { get; private set; }

		public Map Map { get; private set; }

		public Avatar Avatar { get; private set; }

		public static Random Rnd { get; private set; }

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
			m_activeCreatures.AddRange(
				Map.GetBlocksNear(Avatar.Coords).SelectMany(_tuple => _tuple.Item2.Creatures).Union(new[] {Avatar}).Distinct().
					OrderBy(_creature => _creature.BusyTill));
		}

		public void GameUpdated()
		{
			var done = new List<Creature>();
			while (true)
			{
				var creature = m_activeCreatures.FirstOrDefault();
				if (done.Contains(creature)) break;
				done.Add(creature);

				if (creature == null)
				{
					throw new ApplicationException();
				}

				if (creature != Avatar && creature.ActResult != EActResults.NEED_ADDITIONAL_PARAMETERS)
				{
					creature.Thinking();
				}

				var act = creature.NextAct;

				if (act == null)
				{
					break;
				}

				WorldTick = WorldTick < creature.BusyTill ? creature.BusyTill : WorldTick;

				var actResult = creature.DoAct(act);

				if (actResult == EActResults.NEED_ADDITIONAL_PARAMETERS)
				{
					return;
				}

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

		public static void LetItBeeee()
		{
			TheWorld = new World();
		}
	}
}