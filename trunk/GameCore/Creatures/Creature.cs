using System;
using Common.Messages;
using GameCore.Acts;
using GameCore.Objects;
using Graphics;
using Object = GameCore.Objects.Object;

namespace GameCore.Creatures
{
	public abstract class Creature: Object
	{
		private Point m_inBlock;
		protected readonly World m_world;

		/// <summary>
		/// Кидает ли существо сообщения в лог (true для аватара)
		/// </summary>
		protected bool m_silence = true;

		protected Creature(World _world, Point _coords, int _speed)
		{
			m_world = _world;
			Speed = _speed;

			Coords = _coords;
		}

		/// <summary>
		/// Ход в игре с точки зрения существа
		/// Так как скорости не однородны, с точки зрения медленных или быстрых монстров выглядит иначе
		/// </summary>
		public long Turn { get; private set; }

		/// <summary>
		/// Скорость существа, валидно значение >0, множитель, на который умножается время выполнения действия, 100 - нормальная скорость человека
		/// </summary>
		public int Speed { get; private set; }

		private Point m_coords;

		/// <summary>
		/// Мировые координаты
		/// </summary>
		public Point Coords
		{
			get { return m_coords; }
			set
			{
				m_coords = value;
				var newCoords = MapBlock.GetBlockCoords(value);

				if (this is Avatar || newCoords == m_inBlock) return;

				if (m_inBlock != null)m_world.Map.MoveCreature(this, m_inBlock, newCoords);

				m_inBlock = newCoords;
			}
		}

		/// <summary>
		/// Время до которого существо будет выполнять текущее действие
		/// </summary>
		public long BusyTill { get; protected set; }

		public abstract Act GetNextAct();
		
		public void DoAct(Act _act)
		{
			_act.Do(this, m_world, m_silence);
			BusyTill = m_world.WorldTick + _act.TakeTicks*Speed;
			Turn++;
			ActDone();
		}

		protected virtual void ActDone()
		{
		}

		public void ObjectTaken(Object _o)
		{
			//throw new NotImplementedException();
		}
	}
}