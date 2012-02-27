using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	public abstract class Creature : Thing
	{
		private static int m_n;

		protected static Random Rnd = new Random(1);

		/// <summary>
		/// 	Кидает ли существо сообщения в лог (true для аватара)
		/// </summary>
		protected bool Silence = true;

		private Point m_liveCoords;
		private WorldLayer m_layer;

		protected Creature(WorldLayer _layer, int _speed)
		{
			Speed = _speed;
			Luck = 25;
			Nn = m_n++;
			m_layer = _layer;
		}

		public int Nn { get; private set; }

		public void Born(Point _liveCoords)
		{
			m_liveCoords = _liveCoords;
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
		/// Live координаты
		/// </summary>
		public Point LiveCoords
		{
			get { return m_liveCoords; }
			set
			{
				var oldValue = m_liveCoords;
				m_liveCoords = value;
				World.TheWorld.LiveMap.CreaturesCellChanged(this, oldValue, m_liveCoords);
			}
		}

		public LiveMapCell this[Point _point]
		{
			get { return World.TheWorld.LiveMap.GetCell(LiveCoords + _point); }
		}

		public LiveMapCell this[int _x, int _y]
		{
			get { return World.TheWorld.LiveMap.GetCell(LiveCoords + new Point(_x, _y)); }
		}

		public WorldLayer Layer
		{
			get { return m_layer; }
			set
			{
				if (m_layer == value)
				{
					throw new ApplicationException("Лишнее действие");
				}
				var oldLayer = m_layer;
				m_layer = value;
				if (oldLayer != null)
				{
					World.TheWorld.LiveMap.CreaturesLayerChanged(this, oldLayer, value);
				}
			}
		}


		/// <summary>
		/// 	Время до которого существо будет выполнять текущее действие
		/// </summary>
		public long BusyTill { get; protected set; }

		public double GetLuckRandom
		{
			get { return Luck*World.Rnd.NextDouble()/100.0; }
		}

		public int Luck { get; protected set; }

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

		public bool IsAvatar
		{
			get { return World.TheWorld.Avatar == this; }
		}

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
			ActResult = _act.Do(this, Silence);
			var price = _act.TakeTicks*Speed;
			switch (ActResult)
			{
				case EActResults.NOTHING_HAPPENS:
					price = Speed;
					break;
				case EActResults.DONE:
					break;
				case EActResults.FAIL:
					price *= 2;
					break;
				case EActResults.QUICK_FAIL:
					price /= 2;
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

		public virtual EThinkingResult Thinking()
		{
			return EThinkingResult.NORMAL;
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
			var points = LiveCoords.NearestPoints;
			if (_intersect != null && _intersect.Any())
			{
				points = points.Intersect(_intersect);
			}
			return points.Select(World.TheWorld.LiveMap.GetCell).SelectMany(_cell => _cell.GetAllAvailableItemDescriptors(this));
		}

		public virtual IEnumerable<ThingDescriptor> GetBackPackItems()
		{
			yield break;
		}

		protected override int CalcHashCode()
		{
			return base.CalcHashCode() ^ Nn;
		}

		public virtual EActResults Atack(Creature _victim)
		{
			return EActResults.NOTHING_HAPPENS;
		}
	}


	public enum EThinkingResult
	{
		NORMAL,
		/// <summary>
		/// 	Существо самоуничтожилось
		/// </summary>
		SHOULD_BE_REMOVED_FROM_QUEUE,
	}
}