﻿namespace GameCore
{
	public static class Constants
	{
        public const string RESOURCES_PNG_FILE = @"..\resources\resources.png";
        public const string RESOURCES_DB_FILE = @"..\resources\resources.dat";
        public const string RESOURCES_FONT_FILE = @"..\resources\monof55.ttf";

		/// <summary>
        /// Размер блока карты
        /// </summary>
		public const int MAP_BLOCK_SIZE = 32;

		public const int AUTO_MOVE_REPEAT_MILLISECONDS = 50;

		public const int AUTO_MOVE_REPEAT_AFTER = 200;

		/// <summary>
		/// Размер мира в блоках, если 1 - строит тестовую карту, в зависимости от <see cref="WORLD_SEED"/>
		/// </summary>
		public static int WORLD_MAP_SIZE = 32;

		/// <summary>
		/// Сид рандомайзера мира
		/// </summary>
		public static int WORLD_SEED = 2;

		/// <summary>
		/// Размер тайла в пикселях
		/// </summary>
		public static int TILE_SIZE = 32;

		public static bool GAME_MODE = true;
	}
}
