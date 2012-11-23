using System;
using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Battle;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Creatures.Monsters
{
	public abstract class AbstractMonster : Creature
	{
		private int m_skip;

		protected AbstractMonster(WorldLayer _layer, int _speed): base(_layer, _speed)
		{
		}

		public override FColor LerpColor { get; protected set; }

		public override ETileset Tileset { get { return ETileset.MONSTERS; } }

		public override EFraction Fraction
		{
			get { return EFraction.MONSTERS; }
		}

		public EMonsterBehaviour Behaviour { get; set; }
		public override void Resolve(Creature _creature) { }

		public override EThinkingResult Thinking()
		{
			switch (Behaviour)
			{
				case EMonsterBehaviour.IDLE:
					return Idle();
				case EMonsterBehaviour.FOLLOW_AVATAR_INDUNGEON:
					return FollowAvatarIndungeon();
				case EMonsterBehaviour.ATACK_AVATAR:
					return AtackAvatar();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private EThinkingResult Idle()
		{
			if(this[0,0].IsVisibleNow)
			{
				Behaviour=EMonsterBehaviour.ATACK_AVATAR;
				var info = World.TheWorld.BattleProcessor[this];
				info.Agro[World.TheWorld.Avatar] = 1;
			}
			AddActToPool(new WaitAct());
			return EThinkingResult.NORMAL;
		}

		private EThinkingResult AtackAvatar()
		{
			var myLiveCell = this[0, 0];
			var destLiveCell = World.TheWorld.Avatar[0, 0];

			var d = myLiveCell.LiveCoords.GetDistTill(destLiveCell.LiveCoords);
			if (d < 2)
			{
				AddActToPool(new WaitAct());
				return EThinkingResult.NORMAL;
			}
			{
				///Найти путь к одной из соседних клеток рядом с аватаром
				var path = World.TheWorld.LiveMap.PathFinder.FindPath(this, destLiveCell.PathMapCoords.AllNeighbours.ToArray().RandomItem(World.Rnd));
				if (path != null)
				{
					AddActToPool(new MoveToAct(this, path));
					return EThinkingResult.NORMAL;
				}
			}

			{
				var phi = World.Rnd.NextDouble()*Math.PI*2;
				var ro = World.Rnd.NextDouble()*10;
				var dx = (int) (Math.Cos(phi)*ro);
				var dy = (int) (Math.Sin(phi)*ro);

				var path = World.TheWorld.LiveMap.PathFinder.FindPath(this, destLiveCell.PathMapCoords + new Point(dx,dy));
				if (path != null)
				{
					AddActToPool(new MoveToAct(this, path));
					return EThinkingResult.NORMAL;
				}
			}
			AddActToPool(new WaitAct());
			return EThinkingResult.NORMAL;
		}

		private EThinkingResult FollowAvatarIndungeon()
		{
			var myLiveCell = this[0, 0];
			if (m_skip > 0 || !myLiveCell.IsSeenBefore)
			{
				m_skip--;
				AddActToPool(new WaitAct());
				return EThinkingResult.NORMAL;
			}
			var destLiveCell = World.TheWorld.Avatar[0, 0];
			if (destLiveCell.InRoom == myLiveCell.InRoom)
			{
				AddActToPool(new WaitAct());
				return EThinkingResult.NORMAL;
			}

			var path = World.TheWorld.LiveMap.PathFinder.FindPath(this,
			                                                      destLiveCell.PathMapCoords +
			                                                      Point.NearestDPoints.ToArray().RandomItem(World.Rnd));
			if (path != null)
			{
				AddActToPool(new MoveToAct(this, path));
				return EThinkingResult.NORMAL;
			}
			else
			{
				m_skip = 6;
			}

			AddActToPool(new MoveAct(), new Point(World.Rnd.Next(3) - 1, World.Rnd.Next(3) - 1));
			return EThinkingResult.NORMAL;
		}
	}

	public enum EMonsterBehaviour
	{
		IDLE,
		FOLLOW_AVATAR_INDUNGEON,
		ATACK_AVATAR,
	}
}