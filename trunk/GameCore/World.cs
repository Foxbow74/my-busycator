using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore
{
	public class World
	{
		private const int WORLD_SEED = 1;

		/// <summary>
		/// 	содержит список активных в данный момент существ
		/// </summary>
		private readonly List<Creature> m_activeCreatures = new List<Creature>();

		static World()
		{
			Rnd = new Random(WorldSeed);
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

		public static int WorldSeed
		{
			get { return WORLD_SEED; }
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

				#region не давать ходить дважды до перерисовки

				if (done.Contains(creature)) break;
				done.Add(creature);

				#endregion

				if (creature == null)
				{
					throw new ApplicationException();
				}

				if (creature != Avatar && creature.ActResult != EActResults.NEED_ADDITIONAL_PARAMETERS && creature.NextAct==null)
				{
					var thinkingResult = creature.Thinking();
					switch (thinkingResult)
					{
						case EThinkingResult.NORMAL:
							break;
						case EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE:
							m_activeCreatures.Remove(creature);
							continue;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				var act = creature.NextAct;

				if (act == null)
				{
					break;
				}

				WorldTick = WorldTick < creature.BusyTill ? creature.BusyTill : WorldTick;

				var actResult = creature.DoAct(act);

				switch (actResult)
				{
					case EActResults.NOTHING_HAPPENS:
						break;
					case EActResults.DONE:
						break;
					case EActResults.NEED_ADDITIONAL_PARAMETERS:
						return;
					case EActResults.FAIL:
						break;
					case EActResults.SHOULD_BE_REMOVED_FROM_QUEUE:
						m_activeCreatures.Remove(creature);
						continue;
					default:
						throw new ArgumentOutOfRangeException();
				}

				m_activeCreatures.Remove(creature);
				AddToActiveCreatures(creature);
			}
			MessageManager.SendMessage(this, WorldMessage.Turn);
		}

		private void AddToActiveCreatures(Creature _creature)
		{
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

		public static void LetItBeeee()
		{
			TheWorld = new World();
		}

		public void KeyPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var act = KeyTranslator.TranslateKey(_key, _modifiers);
			if (act == null) return;

			Avatar.AddActToPool(act);
		}

		public void AddToBlockAndActiveCreatures(Creature _creature)
		{
			var block = Map.GetMapBlock(_creature.Coords);
			block.Creatures.Add(_creature);
			if (Map.GetBlocksNear(Avatar.Coords).Any(_tuple => _tuple.Item2 == block))
			{
				AddToActiveCreatures(_creature);
			}
		}

		public void RemoveCreature(Creature _creature)
		{
			var block = Map.GetMapBlock(_creature.Coords);
			if (!block.Creatures.Contains(_creature))
			{
				throw new ApplicationException("нет тут таких");
			}
			block.Creatures.Remove(_creature);
		}
	}
}