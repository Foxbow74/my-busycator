﻿#region

using System;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Misc;

#endregion

namespace GameCore.Creatures
{
	public class Avatar : Intelligent
	{
		private Act m_nextAct;

		public Avatar(World _world) : base(_world, Point.Zero, 100)
		{
			m_silence = false;
		}

		public override ETiles Tile
		{
			get { return ETiles.AVATAR; }
		}

		public override string Name
		{
			get { return "аватар"; }
		}

		public void MoveCommandReceived(int _dx, int _dy)
		{
			m_nextAct = new MoveAct(new Point(_dx, _dy));
		}

		public void CommandReceived(ECommands _command)
		{
			switch (_command)
			{
				case ECommands.TAKE:
					m_nextAct = new TakeAct();
					break;
				case ECommands.OPEN:
					m_nextAct = new OpenAct();
					break;
				default:
					throw new ArgumentOutOfRangeException("_command");
			}
		}

		protected override void ActDone()
		{
			m_nextAct = null;
			MessageManager.SendMessage(this, WorldMessage.AvatarTurn);
		}

		public override Act GetNextAct()
		{
			return m_nextAct;
		}
	}
}