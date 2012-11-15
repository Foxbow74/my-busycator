namespace GameCore.Objects.Weapons
{
	public class Axe : Weapon
	{
		public Axe(Material _material) : base(_material) { }

		public override ETiles Tileset { get { return ETiles.AXE; } }

		public override string Name { get { return "топор"; } }
	}
}