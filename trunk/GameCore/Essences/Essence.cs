using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Essences
{
	public abstract class Essence : ITileInfoProvider
	{
		private readonly EALNouns m_eNoun;
		private Noun m_name;

		protected Essence(EALNouns _name, Material _material)
		{
			m_eNoun = _name;
			m_eNoun = _name;
			Material = _material;
			UpdateName();
		}

		protected void UpdateName()
		{
			m_name = GetUpdatedName(m_eNoun.AsNoun());
		}

		protected virtual Noun GetUpdatedName(Noun _noun)
		{
			if (Material != null)
			{
				_noun = _noun + Material.Name;
			}
			return _noun;
		}

		/// <summary>
		/// Непрозрачность объекта, используется для рассчета области видимости игрока
		/// </summary>
		public virtual float Opacity
		{
			get { return TileSetInfoProvider.GetOpacity(Tileset, TileIndex); }
		}

		public virtual ILightSource Light { get { return null; } }

		public abstract EMaterialType AllowedMaterialsType { get; }

		public virtual Material Material { get; private set; }

		#region INameProvider Members

		public ESex Sex { get; protected set; }

		public virtual Noun Name
		{
			get
			{
				return m_name;
			}
		}

		public virtual bool IsCreature
		{
			get { return false; }
		}

		public virtual bool IsUnique
		{
			get { return false; }
		}

		#endregion

		#region ITileInfoProvider Members

		public abstract ETileset Tileset { get; }

		public virtual FColor LerpColor { get { return Material.LerpColor; } protected  set{} }

		public virtual EDirections Direction { get { return EDirections.DOWN; } }

		public virtual bool IsCorpse
		{
			get { return false; }
		}

		public virtual int TileIndex { get; protected set; }

		public EALNouns ENoun
		{
			get { return m_eNoun; }
		}

		#endregion

		public override string ToString()
		{
			return Name.Text;
		}

		public override bool Equals(object _obj) { return GetHashCode() == _obj.GetHashCode(); }

		public virtual bool Is<T>()
		{
			return typeof (T).IsAssignableFrom(GetType());
		}

		public bool Equals(Essence _other) { return GetHashCode() == _other.GetHashCode(); }

		internal virtual Essence Clone(Creature _resolver)
		{
			var clone = (Essence)MemberwiseClone();
			clone.UpdateName();
			return clone;
		}

		public override int GetHashCode()
		{
			if (Material != null)
			{
				return GetType().GetHashCode() ^ Material.GetType().GetHashCode();
			}
			return GetType().GetHashCode();
		}

		protected int GetNativeHashCode()
		{
			return base.GetHashCode();
		}
	}
}