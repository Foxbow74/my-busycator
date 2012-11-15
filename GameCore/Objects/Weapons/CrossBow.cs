namespace GameCore.Objects.Weapons
{
	public class CrossBow : RangedWeapon
	{
		public CrossBow(Material _material) : base(_material) { }

		public override ETiles Tileset { get { return ETiles.CROSSBOW; } }

		public override string Name { get { return "арбалет"; } }
	}
}