using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Acts;
using GameCore.CreatureRoles;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	public abstract class Creature : Thing
	{
		private readonly List<AbstractCreatureRole> m_roles = new List<AbstractCreatureRole>();

		private static int m_n;

		private Point m_liveCoords;
		private WorldLayer m_layer;

		protected Creature(WorldLayer _layer, int _speed)
			: base(ThingHelper.GetMaterial<FlashMaterial>())
		{
			Speed = _speed;
			Luck = 25;
			Nn = m_n++;
			m_layer = _layer;
		}

		public IEnumerable<AbstractCreatureRole> Roles { get { return m_roles; } }

		public void AddRole(AbstractCreatureRole _role)
		{
			m_roles.Add(_role);
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
				m_liveCoords = value==null?null:World.TheWorld.LiveMap.GetCell(value).LiveCoords;
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

		private readonly List<Act> m_actPool = new List<Act>();

		public Act NextAct
		{
			get
			{
				while (m_actPool.Count > 0 && m_actPool[0].IsCancelled)
				{
					m_actPool.RemoveAt(0);
				}
				return m_actPool.Count == 0 ? null : m_actPool[0];
			}
		}

		public EActResults ActResult { get; protected set; }

		public bool IsAvatar
		{
			get { return World.TheWorld.Avatar == this; }
		}

		public void AddActToPool(Act _act, params object[] _params)
		{
			m_actPool.Add(_act);
			foreach (var o in _params)
			{
				_act.AddParameter(o.GetType(), o);
			}
		}

		public void InsertActToPool(Act _act, params object[] _params)
		{
			m_actPool.Insert(0, _act);
			foreach (var o in _params)
			{
				_act.AddParameter(o.GetType(), o);
			}
		}

		public EActResults DoAct()
		{
			var act = m_actPool[0];
			m_actPool.RemoveAt(0);

			using (new Profiler(act.Name))
			{
				ActResult = act.Do(this);	
			}

			var price = Speed; 
			switch (ActResult)
			{
				case EActResults.ACT_REPLACED:
					price = 0;
					break;
				case EActResults.DONE:
					price = act.TakeTicks * Speed;
					break;
				case EActResults.FAIL:
					price = act.TakeTicks * 2 * Speed;
					m_actPool.Clear();
					break;
				case EActResults.QUICK_FAIL:
					price = act.TakeTicks / 2 * Speed;
					m_actPool.Clear();
					break;
				case EActResults.NEED_ADDITIONAL_PARAMETERS:
					if(!IsAvatar)
					{
						throw new ApplicationException("Только действия аватара могут потребовать дополнительные параметры");
					}
					AddActToPool(act);
					return ActResult;
			}
			BusyTill = World.TheWorld.WorldTick + price;
			Turn += price>0?1:0;
			
			return ActResult;
		}

		#endregion

		public abstract EThinkingResult Thinking();

		public IEnumerable<ThingDescriptor> GetAllAvailableItems(IEnumerable<Point> _intersect = null)
		{
			return GetBackPackItems().Concat(GetNotTakenAvailableItems(_intersect));
		}

		public IEnumerable<ThingDescriptor> GetNotTakenAvailableItems(IEnumerable<Point> _intersect = null)
		{
			var points = LiveCoords.NearestPoints;
			if (_intersect != null && _intersect.Any())
			{
				points = points.Intersect(_intersect);
			}
			return points.Select(World.TheWorld.LiveMap.GetCell).SelectMany(_cell => _cell.GetAllAvailableItemDescriptors<Item>(this));
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
			return EActResults.ACT_REPLACED;
		}

		public override EMaterial AllowedMaterials
		{
			get { return EMaterial.FLASH; }
		}

		public void ClearActPool()
		{
			m_actPool.Clear();
		}
	}


	public enum EThinkingResult
	{
		NORMAL,
		/// <summary>
		/// Существо самоуничтожилось
		/// </summary>
		SHOULD_BE_REMOVED_FROM_QUEUE,
	}
}