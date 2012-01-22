using Graphics;

namespace GameCore.Objects
{
	public abstract class Object
	{
		protected Object()
		{
		}

		public abstract Tile Tile { get; }
		public abstract string Name { get; }
	}
}
