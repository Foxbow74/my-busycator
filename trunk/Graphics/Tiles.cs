using Microsoft.Xna.Framework;

namespace Graphics
{
	public static class Tiles
	{
		public static Tile[] GrassTiles = new[]
		                                  	{
												new Tile(3, 2, new Color(30, 50, 30)),
												new Tile(5, 2, new Color(30, 60, 30)),
												new Tile(7, 2, new Color(20, 80, 20)),
												new Tile(12, 2, new Color(20, 100, 20)),
												new Tile(14, 2, new Color(20, 120, 20)),

												new Tile(TextureSet.RR_BRICK_01, 5, 2, new Color(30, 60, 30)),
												new Tile(TextureSet.RR_BRICK_01, 7, 2, new Color(20, 80, 20)),
												new Tile(TextureSet.RR_BRICK_01, 12, 2, new Color(20, 100, 20)),
												new Tile(TextureSet.RR_BRICK_01, 14, 2, new Color(20, 120, 20)),

												new Tile(TextureSet.GP_X16, 7, 2, new Color(20, 80, 20)),
												new Tile(TextureSet.GP_X16, 12, 2, new Color(20, 100, 20)),
												new Tile(TextureSet.GP_X16, 14, 2, new Color(20, 120, 20)),

												new Tile(7, 0, new Color(55, 58, 50)),
												new Tile(TextureSet.GP_X16, 7, 0, new Color(65, 68, 60)),
								};

		public static Tile BrickTile = new Tile(0, 12, Color.DarkRed);
		public static Tile FrameTile = new Tile(0, 12, Color.Green);
		public static Tile DoorTile = new Tile(5, 12, Color.Brown);
		public static Tile HeroTile = new Tile(2, 0, Color.White);

		public static Tile[] MashtoomTiles = new Tile[]
		                                    	{
		                                    		new Tile(5, 0, new Color(20, 160, 20)),
													new Tile(6, 0, new Color(20, 80, 20)),
													new Tile(7, 1, new Color(20, 90, 20)),
													new Tile(8, 1, new Color(20, 120, 90)),
													new Tile(12, 1, Color.Gray),
		                                    	};

		public static Tile SolidTile = new Tile(11 , 13, Color.White);
		
		public static Tile GrowndTile = new Tile(0, 0, new Color(10, 20, 10));

		public static Tile WeaponTile = new Tile(14, 14, Color.SteelBlue);
		public static Tile ChestTile = new Tile(TextureSet.RR_BRICK_01, 2, 9, Color.Gold);

		public static Tile Frame_L = new Tile(TextureSet.GP_X16, 3, 11, Color.Gold);
		public static Tile Frame_R = new Tile(TextureSet.GP_X16, 3, 11, Color.Gold);
		public static Tile Frame_B = new Tile(TextureSet.GP_X16, 4, 12, Color.Gold);
		public static Tile Frame_T = new Tile(TextureSet.GP_X16, 4, 12, Color.Gold);
		public static Tile Frame_BL = new Tile(TextureSet.GP_X16, 0, 12, Color.Gold);
		public static Tile Frame_BR = new Tile(TextureSet.GP_X16, 9, 13, Color.Gold);
		public static Tile Frame_TL = new Tile(TextureSet.GP_X16, 10, 13, Color.Gold);
		public static Tile Frame_TR = new Tile(TextureSet.GP_X16, 15, 11, Color.Gold);
	}
}
