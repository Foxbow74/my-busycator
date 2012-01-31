using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Mapping;
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
			Luck = 5;
			Coords = _coords;
		}

		public override float Opaque
		{
			get { return 0.8f; }
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

		public override EThingCategory Category
		{
			get { throw new NotImplementedException(); }
		}

		#region Act functionality

		private readonly Queue<Act> m_actPool = new Queue<Act>();

		public Act NextAct
		{
			get
			{
				while (m_actPool.Count > 0 && m_actPool.Peek().IsCancelled)
				{
					m_actPool.Dequeue();
				}
				return m_actPool.Count == 0 ? null : m_actPool.Peek();
			}
		}

		public EActResults ActResult { get; protected set; }

		public void AddActToPool(Act _act, params object[] _params)
		{
			m_actPool.Enqueue(_act);
			foreach (var o in _params)
			{
				_act.AddParameter(o.GetType(), o);
			}
		}

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
			m_actPool.Dequeue();
		}

		#endregion

		public virtual void Thinking()
		{
		}


		/// <summary>
		/// 	Возвращает шмотк
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