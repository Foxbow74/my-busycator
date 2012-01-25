using GameCore.Misc;

namespace GameCore.Objects
{
	public abstract class Object
	{
		protected Object()
		{
		}

		public abstract ETiles Tile { get; }
		public abstract string Name { get; }

		public override string ToString()
		{
			return Name;
		}
	}
}
