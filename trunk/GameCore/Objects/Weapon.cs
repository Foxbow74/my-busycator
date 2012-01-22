using Graphics;

namespace GameCore.Objects
{
	public class Weapon : Item
	{
		private readonly Tile m_tile;

		public Weapon()
		{
			m_tile = Tiles.WeaponTile;
		}

		public override Tile Tile { get{return m_tile;}  }

		public override string Name
		{
			get { return "оружие"; }
		}
	}
}