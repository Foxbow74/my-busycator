using System;
using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore.Objects
{
	public interface ITileInfoProvider
	{
		ETiles Tile { get; }
		FColor LerpColor { get; }
	}

	public class TileInfoProvider:ITileInfoProvider
	{
		public static ITileInfoProvider HeapOfItems { get; private set; }

		static TileInfoProvider()
		{
			HeapOfItems = new TileInfoProvider(ETiles.HEAP_OF_ITEMS);
		}
		
		public TileInfoProvider(ETiles _tile):this(_tile, FColor.Empty)
		{}

		public TileInfoProvider(ETiles _tile, FColor _colorMultiplier)
		{
			Tile = _tile;
			LerpColor = _colorMultiplier;
		}

		public ETiles Tile { get; private set; }

		public FColor LerpColor { get; private set; }
	}

	public abstract class Thing : ITileInfoProvider
	{
		public abstract ETiles Tile { get; }

		public virtual FColor LerpColor { get{ return FColor.Empty;}}

		public abstract string Name { get; }

		public virtual float Opacity
		{
			get { return TilesAttribute.GetAttribute(Tile).Opacity; }
		}

		public abstract EThingCategory Category { get; }

		public override string ToString()
		{
			return Name;
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
			return GetType().GetHashCode();
		}

		public LightSource Light { get; protected set; }
	}
}