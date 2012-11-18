namespace GameCore
{
	public static class Constants
	{
        public const string RESOURCES_PNG_FILE = @"..\resources\resources.png";
        public const string RESOURCES_DB_FILE = @"..\resources\resources.dat";
        public const string RESOURCES_FONT_FILE = @"..\resources\monof55.ttf";

        /// <summary>
        /// Размер мира в блоках, если 1 - строит тестовую карту, в зависимости от <see cref="WORLD_SEED"/>
        /// </summary>
		public static int WORLD_MAP_SIZE = 1;

        /// <summary>
        /// Сид рандомайзера мира
        /// </summary>
		public static int WORLD_SEED = 1;

        /// <summary>
        /// Размер блока карты
        /// </summary>
		public const int MAP_BLOCK_SIZE = 32;

        /// <summary>
        /// Размер тайла в пикселях
        /// </summary>
		public static int TILE_SIZE = 16;

		public const int AUTO_MOVE_REPEAT_MILLISECONDS = 50;

		public const int AUTO_MOVE_REPEAT_AFTER = 200;
	}
}
