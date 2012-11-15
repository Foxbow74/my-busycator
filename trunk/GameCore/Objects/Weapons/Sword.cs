namespace GameCore.Objects.Weapons
{
	public class Sword : Weapon
	{
		public Sword(Material _material) : base(_material) { }

		public override ETileset Tileset { get { return ETileset.SWORD; } }

		public override string Name { get { return "меч"; } }
	}
}