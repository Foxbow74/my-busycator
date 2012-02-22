using System;
using System.Collections.Generic;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore
{
	public class World
	{
		private const int WORLD_SEED = 1;

		/// <summary>
		/// 	содержит список активных в данный момент существ
		/// </summary>

		private readonly List<WorldLayer> m_layers = new List<WorldLayer>();

		static World()
		{
			Rnd = new Random(WorldSeed);
		}

		public World()
		{
			LiveMap = new LiveMap();
			m_layers.Add(Surface = new Surface());

			WorldTick = 0;
		}

		public static World TheWorld { get; private set; }

		public long WorldTick { get; private set; }

		public Avatar Avatar { get; private set; }

		public Point AvatarBlockId { get; private set; }

		public static Random Rnd { get; private set; }

		public static int WorldSeed
		{
			get { return WORLD_SEED; }
		}

		public Surface Surface { get; private set; }

		public LiveMap LiveMap { get; private set; }

		private void BornAvatar()
		{
			AvatarBlockId = Point.Zero;
			Avatar = new Avatar(Surface) { LiveCoords = LiveMap.CenterLiveBlock * MapBlock.SIZE };
			LiveMap.Actualize();
		}

		/// <summary>
		/// </summary>
		/// <returns>true, if something changed</returns>
		public bool GameUpdated()
		{
			var result = false;
			var done = new List<Creature>();
			while (true)
			{
				var creature = LiveMap.FirstActiveCreature;

				#region не давать ходить дважды до перерисовки

				if (done.Contains(creature)) break;
				done.Add(creature);

				#endregion

				if (creature == null)
				{
					throw new ApplicationException();
				}

				if ((!creature.IsAvatar) && creature.ActResult != EActResults.NEED_ADDITIONAL_PARAMETERS && creature.NextAct == null)
				{
					var thinkingResult = creature.Thinking();
					switch (thinkingResult)
					{
						case EThinkingResult.NORMAL:
							break;
						case EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE:
							//m_activeCreatures.Remove(creature);
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
					case EActResults.NEED_ADDITIONAL_PARAMETERS:
						return true;
					case EActResults.NOTHING_HAPPENS:
						break;
					case EActResults.DONE:
					case EActResults.FAIL:
					case EActResults.QUICK_FAIL:
						result = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			if(result)
			{
				MessageManager.SendMessage(this, WorldMessage.Turn);
			}
			return result;
		}

		public static void LetItBeeee()
		{
			TheWorld = new World();
			TheWorld.BornAvatar();
		}

		public void KeyPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var act = KeyTranslator.TranslateKey(_key, _modifiers);
			if (act == null) return;

			Avatar.AddActToPool(act);
		}

		internal WorldLayer GenerateNewLayer(Creature _creature, Stair _stair)
		{
			return new TreeMazeDungeonLayer(_creature.Layer, _creature[0, 0].WorldCoords, _stair);
			
			var rnd = new Random(_creature.BlockRandomSeed);
			switch (rnd.Next(2))
			{
				case 0:
					return new DungeonLayer(_creature.Layer, _creature.LiveCoords, _stair);
				case 1:
					return new TreeMazeDungeonLayer(_creature.Layer, _creature[0,0].WorldCoords, _stair);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public  void SetAvatarBlockId(Point _newBlockId)
		{
			AvatarBlockId = _newBlockId;
		}
	}
}