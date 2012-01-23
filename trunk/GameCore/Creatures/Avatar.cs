using System;
using Common.Messages;
using GameCore.Acts;
using Graphics;

namespace GameCore.Creatures
{
	public class Avatar : Creature
	{
		public Avatar(World _world): base(_world, Point.Zero, 100)
		{
			m_silence = false;
		}

		public void MoveCommandReceived(int _dx, int _dy)
		{
			m_nextAct = new MoveAct(new Point(_dx, _dy));
		}

		public void CommandReceived(ECommands _command)
		{
			switch (_command)
			{
				case ECommands.INVENTORY:
					break;
				case ECommands.TAKE:
					m_nextAct = new TakeAct();
					break;
				default:
					throw new ArgumentOutOfRangeException("_command");
			}
		}

		private Act m_nextAct;

		protected override void ActDone()
		{
			m_nextAct = null;
		}

		public override Act GetNextAct()
		{
			return m_nextAct;
		}

		public override Tile Tile
		{
			get { return Tiles.Avatar; }
		}

		public override string Name
		{
			get { return "аватар"; }
		}
	}
}