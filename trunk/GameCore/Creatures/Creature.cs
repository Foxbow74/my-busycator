#region

using System;
using GameCore.Acts;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

#endregion

namespace GameCore.Creatures
{
	public abstract class Creature : Thing
	{
		protected static Random m_rnd = new Random(1);

		private Point m_coords;
		private Point m_inBlock;

		/// <summary>
		/// 	Кидает ли существо сообщения в лог (true для аватара)
		/// </summary>
		protected bool m_silence = true;

		protected Creature(Point _coords, int _speed)
		{
			Speed = _speed;
			Luck = 100;
			Coords = _coords;
		}

		/// <summary>
		/// 	Ход в игре с точки зрения существа
		/// 	Так как скорости не однородны, с точки зрения медленных или быстрых монстров выглядит иначе
		/// </summary>
		public long Turn { get; private set; }

		/// <summary>
		/// 	Скорость существа, валидно значение >0, множитель, на который умножается время выполнения действия, 100 - нормальная скорость человека
		/// </summary>
		public int Speed { get; private set; }

		/// <summary>
		/// 	Мировые координаты
		/// </summary>
		public Point Coords
		{
			get { return m_coords; }
			set
			{
				var newCoords = MapBlock.GetBlockCoords(value);
				if (newCoords != m_inBlock)
				{
					if (!(this is Avatar) && m_inBlock != null)
					{
						Map.MoveCreature(this, m_inBlock, newCoords);
					}
					m_inBlock = newCoords;
				}

				m_coords = value;
			}
		}

		/// <summary>
		/// 	Время до которого существо будет выполнять текущее действие
		/// </summary>
		public long BusyTill { get; protected set; }

		public int GetLuckRandom
		{
			get { return World.Rnd.Next(Luck); }
		}

		public int Luck { get; protected set; }

		public Act NextAct { get; protected set; }

		public void DoAct(Act _act)
		{
			_act.Do(this, m_silence);
			BusyTill = World.TheWorld.WorldTick + _act.TakeTicks * Speed;
			Turn++;
			ActDone();
		}

		protected virtual void ActDone()
		{
			NextAct = null;

			if (this == World.TheWorld.Avatar)
			{
				MessageManager.SendMessage(this, WorldMessage.AvatarTurn);
			}
		}

		public MapBlock MapBlock
		{
			get { return World.TheWorld.Map[m_inBlock]; }
		}

		public MapCell MapCell
		{
			get { return Map.GetMapCell(Coords); }
		}

		public void MoveCommandReceived(int _dx, int _dy)
		{
			NextAct = new MoveAct(new Point(_dx, _dy));
		}

		public void CommandReceived(ECommands _command)
		{
			switch (_command)
			{
				case ECommands.TAKE:
					NextAct = new TakeAct();
					break;
				case ECommands.OPEN:
					NextAct = new OpenAct();
					break;
				default:
					throw new ArgumentOutOfRangeException("_command");
			}
		}

		public virtual void Thinking()
		{
			
		}
	}
}