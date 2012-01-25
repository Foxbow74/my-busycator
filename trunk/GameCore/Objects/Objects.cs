namespace GameCore.Objects
{
	public abstract class Thing
	{
		public abstract ETiles Tile { get; }
		public abstract string Name { get; }

		public override string ToString()
		{
			return Name;
		}
	}
}