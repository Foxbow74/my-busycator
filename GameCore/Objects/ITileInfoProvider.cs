namespace GameCore.Objects
{
	public interface ITileInfoProvider
	{
		ETiles Tile { get; }
		FColor LerpColor { get; }
		EDirections Direction { get; }
	}
}