using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

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

		public MapBlock MapBlock
		{
			get { return World.TheWorld.Map[m_inBlock]; }
		}

		public MapCell MapCell
		{
			get { return Map.GetMapCell(Coords); }
		}

		#region Act functionality

		private readonly Queue<Act> m_actPool = new Queue<Act>();

		public void AddActToPool(Act _act)
		{
			m_actPool.Enqueue(_act);
		}

		public Act NextAct
		{
			get
			{
				while (m_actPool.Count>0 && m_actPool.Peek().IsCancelled)
				{
					m_actPool.Dequeue();
				}
				return m_actPool.Count == 0 ? null : m_actPool.Peek();
			}
		}

		public EActResults ActResult { get; protected set; }

		public EActResults DoAct(Act _act)
		{
			ActResult = _act.Do(this, m_silence);
			var price = _act.TakeTicks*Speed;
			switch (ActResult)
			{
				case EActResults.NOTHING_HAPPENS:
					price /= 2;
					break;
				case EActResults.DONE:
					break;
				case EActResults.FAIL:
					price *= 2;
					break;
				case EActResults.NEED_ADDITIONAL_PARAMETERS:
					return ActResult;
			}
			BusyTill = World.TheWorld.WorldTick + price;
			Turn++;
			ActDone();
			return ActResult;
		}

		protected virtual void ActDone()
		{
			var act = m_actPool.Dequeue();
			if (!act.IsCancelled && this == World.TheWorld.Avatar)
			{
				MessageManager.SendMessage(this, WorldMessage.AvatarTurn);
			}
		}

		#endregion

		public void MoveCommandReceived(Point _dPoint)
		{
			AddActToPool(new MoveAct(_dPoint));
		}

		public void CommandReceived(ECommands _command)
		{
			switch (_command)
			{
				case ECommands.TAKE:
					AddActToPool(new TakeAct());
					break;
				case ECommands.OPEN:
					AddActToPool(new OpenAct());
					break;
				default:
					throw new ArgumentOutOfRangeException("_command");
			}
		}

		public virtual void Thinking()
		{
		}


		/// <summary>
		/// Возвращает шмотк
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ThingDescriptor> GetAllAvailableItems(IEnumerable<Point> _intersect = null)
		{
			return GetBackPackItems().Union(GetNotTakenAvailableItems(_intersect));
		}

		public IEnumerable<ThingDescriptor> GetNotTakenAvailableItems(IEnumerable<Point> _intersect = null)
		{
			var points = Coords.NearestPoints;
			if (_intersect != null && _intersect.Any())
			{
				points = points.Intersect(_intersect);
			}
			return points.Select(Map.GetMapCell).SelectMany(_cell => _cell.GetAllAvailableItems(this));
		}

		public virtual IEnumerable<ThingDescriptor> GetBackPackItems()
		{
			yield break;
		}
	}
}