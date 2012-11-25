namespace GameCore.Essences
{
	public interface ITileInfoProvider
	{
		ETileset Tileset { get; }

		/// <summary>
		/// Цвет, который дает конкретный предмет своему тайлу. Накладывается на базовый цвет тайла линейной интерполяцией с учетом альфы
		/// </summary>
		FColor LerpColor { get; }

		/// <summary>
		/// Позволяет поворачивать тайл при отрисовке, по дефолту <see cref="EDirections.DOWN"/>
		/// </summary>
		EDirections Direction { get; }

		/// <summary>
		/// Позволяет поворачивать тайл при отрисовке, по дефолту <see cref="EDirections.DOWN"/>
		/// </summary>
		bool IsCorpse { get; }

        /// <summary>
        /// Указывает конкретный тайл в тайлсете
        /// </summary>
		int TileIndex { get; }
	}
}