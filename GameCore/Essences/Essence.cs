﻿using GameCore.Creatures;
using GameCore.Misc;
using RusLanguage;

namespace GameCore.Essences
{
	public abstract class Essence : ITileInfoProvider, INameProvider
	{
		protected Essence(Material _material) { Material = _material; }

		/// <summary>
		/// Непрозрачность объекта, используется для рассчета области видимости игрока
		/// </summary>
		public virtual float Opacity
		{
			get { return TileSetInfoProvider.GetOpacity(Tileset, TileIndex); }
		}

		public virtual ILightSource Light { get { return null; } }

		public abstract EMaterial AllowedMaterials { get; }

		public virtual Material Material { get; private set; }

		public string this[EPadej _padej] { get { return this[_padej, World.TheWorld.Avatar]; } }

		public string this[EPadej _padej, Creature _creature] { get { return Sklonenia.ToPadej(_padej, this.GetName(_creature), this is Creature, Sex); } }

		#region INameProvider Members

		public ESex Sex { get; protected set; }

		public abstract string Name { get; }

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

		public virtual int TileIndex
		{
			get { return 0; }
		}

		#endregion

		public override string ToString() { return this.GetName(World.TheWorld.Avatar); }

		/// <summary>
		/// Заполнить параметры сущности, происходит один раз для сущности
		/// </summary>
		/// <param name = "_creature">Существо, инициатор резолва</param>
		public abstract void Resolve(Creature _creature);

		public override bool Equals(object _obj) { return GetHashCode() == _obj.GetHashCode(); }

		public virtual bool Is<T>()
		{
			return typeof (T).IsAssignableFrom(GetType());
		}

		public bool Equals(Essence _other) { return GetHashCode() == _other.GetHashCode(); }

		public override int GetHashCode() { return CalcHashCode(); }

		protected virtual int CalcHashCode() { return GetType().GetHashCode() ^ (Material == null ? 0 : Material.GetHashCode()); }
		
		public virtual string GetFullName() { return Name + " из " + Material[EPadej.ROD]; }
	}
}