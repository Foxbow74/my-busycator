namespace GameCore.Essences
{
	public interface ITileInfoProvider
	{
		ETileset Tileset { get; }

		/// <summary>
		/// Цвет, который дает конкретный предмет своему тайлу. Считается линейной интерполяцией от альфы
		/// </summary>
		FColor LerpColor { get; }

		/// <summary>
		/// Позволяет поворачивать тайл при отрисовке, по дефолту <see cref="EDirections.DOWN"/>
		/// </summary>
		EDirections Direction { get; }

		int TileIndex { get; }
	}
}