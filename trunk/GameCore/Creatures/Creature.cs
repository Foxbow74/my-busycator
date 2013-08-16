using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Battle;
using GameCore.CreatureRoles;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Mapping;
using GameCore.Mapping.Layers;
using GameCore.Materials;
using GameCore.Misc;

namespace GameCore.Creatures
{
	public abstract class Creature : Essence
	{
		private static int m_identifierCounter = 0;
		private int m_identifier;

		public override bool IsCreature
		{
			get
			{
				return true;
			}
		}

		#region Fields

		private readonly List<AbstractCreatureRole> m_roles = new List<AbstractCreatureRole>();

		private WorldLayer m_layer;

		#endregion

		#region .ctor

		protected Creature(EALNouns _name, WorldLayer _layer, int _speed)
			: base(_name, EssenceHelper.GetMaterial<BodyMaterial>())
		{
			Speed = _speed;
			Luck = 25;
			m_layer = _layer;
			//m_hc = base.GetHashCode();// GetType().GetHashCode() ^ m_identifier;
			UpdateIdentifier();
		}

		#endregion

		#region Methods

		public void AddRole(AbstractCreatureRole _role)
		{
			m_roles.Add(_role);
		}

		public void ClearActPool()
		{
			m_actPool.Clear();
		}

		public IEnumerable<EssenceDescriptor> GetAllAvailableItems(IEnumerable<Point> _intersect)
		{
			return GetBackPackItems().Concat(GetNotTakenAvailableItems(_intersect.ToArray()));
		}

		public virtual IEnumerable<EssenceDescriptor> GetBackPackItems()
		{
			yield break;
		}

		public EssenceDescriptor[] GetNotTakenAvailableItems(Point[] _intersect = null)
		{
			var points = Point.NearestDPoints.AsEnumerable();
			if (_intersect != null && _intersect.Length > 0)
			{
				points = points.Intersect(_intersect);
			}
			return points.Select(_point => this[_point]).SelectMany(_cell => _cell.GetAllAvailableItemDescriptors<Item>(this)).ToArray();
		}

		public abstract EThinkingResult Thinking();

		#endregion

		#region Properties

		public override EMaterialType AllowedMaterialsType
		{
			get { return EMaterialType.BODY; }
		}

		/// <summary>
		/// Время до которого существо будет выполнять текущее действие
		/// </summary>
		public long BusyTill { get; protected set; }

		public double GetLuckRandom
		{
			get { return Luck*World.Rnd.NextDouble()/100.0; }
		}

		#endregion

		#region Act functionality

		private readonly List<Act> m_actPool = new List<Act>();
		private readonly int m_hc;

		public EActResults ActResult { get; protected set; }

		public bool IsAvatar
		{
			get { return World.TheWorld.Avatar == this; }
		}

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

		public void AddActToPool(Act _act, params object[] _params)
		{
			m_actPool.Add(_act);
			foreach (var o in _params)
			{
				_act.AddParameter(o.GetType(), o);
			}
		}

		public EActResults DoAct()
		{
			var act = m_actPool[0];
			m_actPool.RemoveAt(0);

			using (new Profiler(act.Name.GetString()))
			{
				ActResult = act.Do(this);
			}

			var price = Speed;
			switch (ActResult)
			{
				case EActResults.ACT_REPLACED:
					price = 0;
					break;
				case EActResults.WORLD_STAYS_UNCHANGED:
				case EActResults.DONE:
					price = act.TakeTicks*Speed;
					break;
				case EActResults.FAIL:
					price = act.TakeTicks*2*Speed;
					m_actPool.Clear();
					break;
				case EActResults.QUICK_FAIL:
					price = act.TakeTicks/2*Speed;
					m_actPool.Clear();
					break;
				case EActResults.NEED_ADDITIONAL_PARAMETERS:
					if (!IsAvatar)
					{
						throw new ApplicationException("Только действия аватара могут потребовать дополнительные параметры");
					}
					AddActToPool(act);
					return ActResult;
			}

			//MessageManager.SendMessage(this, new SimpleTextMessage(EMessageType.DEBUG, string.Format("{0}({3}) {1} takes {2}", Name, act.Name, price, GetHashCode())));

			BusyTill = World.TheWorld.WorldTick + price;
			Turn += price > 0 ? 1 : 0;

			return ActResult;
		}

		public void InsertActToPool(Act _act, params object[] _params)
		{
			m_actPool.Insert(0, _act);
			foreach (var o in _params)
			{
				_act.AddParameter(o.GetType(), o);
			}
		}

		#endregion

		#region Properties

		public LiveMapCell this[Point _point]
		{
			get { return GeoInfo[_point]; }
		}

		public LiveMapCell this[int _x, int _y]
		{
			get { return GeoInfo[_x, _y]; }
		}
		
		public int Luck { get; protected set; }
		public int Nn { get { return m_identifier; } }

		public IEnumerable<AbstractCreatureRole> Roles
		{
			get { return m_roles; }
		}

		/// <summary>
		/// 	Скорость существа, валидно значение >0, множитель, на который умножается время выполнения действия, 100 - нормальная скорость человека
		/// </summary>
		public int Speed { get; protected set; }

		/// <summary>
		/// 	Ход в игре с точки зрения существа Так как скорости не однородны, с точки зрения медленных или быстрых монстров выглядит иначе
		/// </summary>
		public long Turn { get; private set; }

		#endregion

		public abstract EFraction Fraction { get; }

		public CreatureGeoInfo GeoInfo { get; set; }

		public abstract CreatureBattleInfo CreateBattleInfo();

		/// <summary>
		/// Получить список оружия против определенного противника
		/// </summary>
		/// <param name="_against"></param>
		/// <returns></returns>
		public abstract IEnumerable<IWeapon> GetWeapons(Creature _against);

		public override int GetHashCode()
		{
			return GetNativeHashCode();
		}

		internal override Essence Clone(Creature _resolver)
		{
			var result = base.Clone(_resolver);
			((Creature)result).UpdateIdentifier();
			return result;
		}

		private void UpdateIdentifier()
		{
			m_identifier = ++m_identifierCounter;
		}
	}
}