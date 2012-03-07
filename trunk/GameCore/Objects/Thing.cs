using System.Collections.Generic;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Objects
{
	public abstract class Thing : ITileInfoProvider
	{
		protected Thing(Material _material)
		{
			Material = _material;
		}

		public abstract ETiles Tile { get; }

		public virtual FColor LerpColor { get{ return Material.LerpColor; }}

		public virtual EDirections Direction
		{
			get { return EDirections.DOWN; }
		}

		public abstract string Name { get; }

		public virtual float Opacity
		{
			get { return TilesAttribute.GetAttribute(Tile).Opacity; }
		}

		public abstract EThingCategory Category { get; }

		public override string ToString()
		{
			return this.GetName(World.TheWorld.Avatar);
		}

		/// <summary>
		/// 	Заполнить параметры вещи, происходит один раз
		/// </summary>
		/// <param name = "_creature"></param>
		public abstract void Resolve(Creature _creature);

		public override bool Equals(object _obj)
		{
			return GetHashCode() == _obj.GetHashCode();
		}

		public virtual bool Is<T>()
		{
			return typeof(T).IsAssignableFrom(GetType());
		}

		public bool Equals(Thing _other)
		{
			return GetHashCode() == _other.GetHashCode();
		}

		public override int GetHashCode()
		{
			return CalcHashCode();
		}

		protected virtual int CalcHashCode()
		{
			return GetType().GetHashCode() ^ (Material==null?0:Material.GetHashCode());
		}

		public virtual ILightSource Light { get { return null; } }

		public abstract IEnumerable<EMaterial> AllowedMaterials { get; }

		public virtual Material Material { get; private set; }
	}
}