﻿using System;
using System.Collections.Generic;
using System.IO;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Essences.Things;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Storage;
using GameCore.Storeable;

namespace GameCore
{
	public class World
	{
// ReSharper disable NotAccessedField.Local
		static XResourceServer m_resourceSrv;
// ReSharper restore NotAccessedField.Local

		static XResourceClient m_resourceCli;

		private readonly Dictionary<uint, Tuple<IRemoteActivation, Point>> m_remoteActivation = new Dictionary<uint, Tuple<IRemoteActivation, Point>>();

		/// <summary>
		/// содержит список активных в данный момент существ
		/// </summary>
		private readonly List<WorldLayer> m_layers = new List<WorldLayer>();

		static World() 
		{
			Rnd = new Random(Constants.WORLD_SEED);

            if (!File.Exists(Constants.RESOURCES_DB_FILE))
            {
                throw new ApplicationException("Не найдена база ресурсов " + Path.GetFullPath(Constants.RESOURCES_DB_FILE));
            }
		}

		public World()
		{
			LiveMap = new LiveMap();
			m_layers.Add(Surface = new Surface());

			WorldTick = 0;
		}

		private static XResourceClient XClient
        {get
        {
            if(m_resourceCli==null)
            {
                m_resourceSrv = new XResourceServer();
                m_resourceCli = new XResourceClient();
            }
            return m_resourceCli;
        }}

        internal static XResourceRoot XResourceRoot { get { return XClient.GetRoot<XResourceRoot>(); } }

		public static World TheWorld { get; private set; }

		public long WorldTick { get; private set; }

		public Avatar Avatar { get; private set; }

		public Point AvatarBlockId { get; private set; }

		public static Random Rnd { get; private set; }

		public Surface Surface { get; private set; }

		public LiveMap LiveMap { get; private set; }

		private void BornAvatar()
		{
			
			Avatar = new Avatar(Surface);
			AvatarBlockId = Surface.City.CityBlockIds[0];// -new Point(Surface.WORLD_MAP_SIZE / 2, Surface.WORLD_MAP_SIZE / 2);
			LiveMap.Actualize();
			Avatar.LiveCoords = Point.Zero;
			
		}

		/// <summary>
		/// </summary>
		/// <returns>true, if something changed</returns>
		public bool GameUpdated(bool _forceTurn = false)
		{
			var result = false;
			var done = new List<Creature>();
			while (true)
			{
				var creature = LiveMap.FirstActiveCreature;

				#region не давать ходить дважды до перерисовки);

				if (done.Contains(creature)) break;
				done.Add(creature);

				#endregion

				if (creature == null)
				{
					throw new ApplicationException();
				}

				if ((!creature.IsAvatar) && creature.ActResult != EActResults.NEED_ADDITIONAL_PARAMETERS &&
				    creature.NextAct == null)
				{
					var thinkingResult = creature.Thinking();
					switch (thinkingResult)
					{
						case EThinkingResult.NORMAL:
							break;
						case EThinkingResult.SHOULD_BE_REMOVED_FROM_QUEUE:
							creature.LiveCoords = null;
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
							return true;
						case EActResults.ACT_REPLACED:
							break;
						case EActResults.DONE:
						case EActResults.FAIL:
						case EActResults.QUICK_FAIL:
							result = true;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				} while (actResult == EActResults.ACT_REPLACED);
			}
			if (result || _forceTurn)
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
			layer.AddStair(_creature.Layer, _creature[0, 0].WorldCoords, _stair);
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