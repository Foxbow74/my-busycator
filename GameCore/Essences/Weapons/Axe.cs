namespace GameCore.Essences.Weapons
{
	public class Axe : Weapon
	{
		public Axe(Material _material) : base(_material) { }

		public override ETileset Tileset { get { return ETileset.AXE; } }

		public override string Name { get { return "топор"; } }
	}
}