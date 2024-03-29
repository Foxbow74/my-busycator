﻿using System;
using System.Collections.Generic;
using System.IO;
using GameCore.Acts;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Things;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Storage;
using GameCore.Storage.XResourceEssences;
using GameCore.Storeable;
using XTransport.Server;

namespace GameCore
{
	public class World
	{
		static readonly XResourceServer m_xServer = new XResourceServer();

		static XResourceClient m_resourceCli;

		private readonly List<WorldLayer> m_layers = new List<WorldLayer>();

		private readonly Dictionary<uint, Tuple<IRemoteActivation, Point>> m_remoteActivation = new Dictionary<uint, Tuple<IRemoteActivation, Point>>();
        private static Type m_startingLayerType;

	    static World() 
		{
            if (!File.Exists(Constants.RESOURCES_DB_FILE))
            {
                throw new ApplicationException("Не найдена база ресурсов " + Path.GetFullPath(Constants.RESOURCES_DB_FILE));
            }
		}

	    public static void SetStartingLayerType<TWorldLayer>() where TWorldLayer : WorldLayer
	    {
	        m_startingLayerType = typeof (TWorldLayer);
	    }

		public World()
		{
			Rnd = new Random(Constants.WORLD_SEED);

			BattleProcessor = new BattleProcessor();
			CreatureManager = new CreatureManager();

			LiveMap = new LiveMap();

			if (Constants.GAME_MODE)
			{
				if (m_startingLayerType == null)
				{
					throw new ApplicationException("Нужно задать тип стартового уровня");
				}
				m_layers.Add(CurrentLayer = (WorldLayer) Activator.CreateInstance(m_startingLayerType));
			}
			WorldTick = 0;
		}

		public BattleProcessor BattleProcessor { get; private set; }
		public CreatureManager CreatureManager { get; private set; }

		public static IAbstractLanguageProcessor AL { get; private set; } 

		private static XResourceClient XClient
		{
			get
			{
				if (m_resourceCli == null)
				{
					m_resourceCli = new XResourceClient();
					XResourceEssenceDummy.Client = m_resourceCli;
				}

				return m_resourceCli;
			}
		}

		internal static XResourceRoot XResourceRoot
		{
			get
			{
				return XClient.GetRoot<XResourceRoot>();
			}
		}

		public static World TheWorld { get; private set; }

		public long WorldTick { get; private set; }

		public Avatar Avatar { get; private set; }

		public Point AvatarBlockId { get; private set; }

		public static Random Rnd { get; private set; }

        public WorldLayer CurrentLayer { get; private set; }

		public LiveMap LiveMap { get; private set; }

		private void BornAvatar()
		{
			if (Constants.GAME_MODE)
			{
				Avatar = new Avatar(CurrentLayer);
				AvatarBlockId = CurrentLayer.GetAvatarStartingBlockId();
				CreatureManager.AddCreature(Avatar, AvatarBlockId*Constants.MAP_BLOCK_SIZE, Point.Zero, CurrentLayer);
				LiveMap.Actualize();
			}
		}

		public void GameUpdated()
		{
			if(WorldTick==0)
			{
				WorldTick = 1;
			}
			var done = new List<Creature>();
			while (true)
			{
				var creature = CreatureManager.FirstActiveCreature;

				#region не давать ходить дважды до перерисовки);

				if (done.Contains(creature)) break;
				
				if(creature.Speed>0)done.Add(creature);

				#endregion

				if ((!creature.IsAvatar) && creature.ActResult != EActResults.NEED_ADDITIONAL_PARAMETERS &&
				    creature.NextAct == null)
				{
					var thinkingResult = creature.Thinking();
					switch (thinkingResult)
					{
						case EThinkingResult.NORMAL:
							break;
						case EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE:
							CreatureManager.CreatureIsDead(creature);
							continue;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				if (creature.NextAct == null)
				{
					break;
				}

				WorldTick = WorldTick < creature.BusyTill ? creature.BusyTill : WorldTick;

				EActResults actResult;

				if (creature.IsAvatar)
				{
					MessageManager.SendMessage(this, WorldMessage.AvatarBeginsTurn);
				}
				do
				{
					actResult = creature.DoAct();

					switch (actResult)
					{
						case EActResults.NEED_ADDITIONAL_PARAMETERS:
							return;
						case EActResults.ACT_REPLACED:
							break;
						case EActResults.DONE:
							break;
						case EActResults.WORLD_STAYS_UNCHANGED:
						case EActResults.FAIL:
						case EActResults.QUICK_FAIL:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				} while (actResult == EActResults.ACT_REPLACED);
				MessageManager.SendMessage(this, WorldMessage.MicroTurn);
			}
			if (done.Contains(Avatar) || Avatar.NextAct == null)
			{
				MessageManager.SendMessage(this, WorldMessage.Turn);
			}
		}

	    public void UpdateDPoint()
	    {
	        DPoint = TheWorld.LiveMap.GetData();
	    }

		public Point DPoint { get; private set; }

		public AbstractXServer XServer
		{
			get { return m_xServer; }
		}

		public static void LetItBeeee(IAbstractLanguageProcessor _abstractLanguageProcessor)
		{
			AL = _abstractLanguageProcessor;

			TheWorld = new World();
			TheWorld.BornAvatar();
		}

		public void KeyPressed(ConsoleKey _key, EKeyModifiers _modifiers=EKeyModifiers.NONE)
		{
			var act = KeyTranslator.TranslateKey(_key, _modifiers);
			if (act == null) return;

			Avatar.AddActToPool(act);
		}

		internal WorldLayer GenerateNewLayer(Creature _creature, Stair _stair)
		{
			var rnd = new Random(_creature[0, 0].LiveMapBlock.MapBlock.RandomSeed);
			var n = rnd.Next();
			WorldLayer layer;
			switch (n%1)
			{
				case 0:
					layer = new TreeMazeDungeonLayer(_creature[0, 0].WorldCoords, rnd);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			layer.AddStair(_creature.GeoInfo.Layer, _creature[0, 0].WorldCoords, _stair);
			return layer;
		}

		public void SetAvatarBlockId(Point _newBlockId) { AvatarBlockId = _newBlockId; }

		public void RegisterRemoteActivation(uint _mechanismId, IRemoteActivation _mechanism, Point _worldCoords)
		{
			m_remoteActivation.Add(_mechanismId, new Tuple<IRemoteActivation, Point>(_mechanism, _worldCoords));
		}

		public Tuple<IRemoteActivation, Point> GetRemoteActivation(uint _mechanismId)
		{
			return m_remoteActivation[_mechanismId];
		}
	}
}