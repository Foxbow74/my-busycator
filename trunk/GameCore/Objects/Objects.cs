using GameCore.Creatures;

namespace GameCore.Objects
{
	public abstract class Thing
	{
		public abstract ETiles Tile { get; }
		public abstract string Name { get; }

		public virtual float Opaque
		{
			get { return 1; }
		}

		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Заполнить параметры вещи, происходит один раз
		/// </summary>
		/// <param name="_creature"></param>
		public abstract void Resolve(Creature _creature);
	}
}