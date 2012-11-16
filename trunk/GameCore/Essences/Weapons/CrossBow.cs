namespace GameCore.Essences.Weapons
{
	public class CrossBow : RangedWeapon
	{
		public CrossBow(Material _material) : base(_material) { }

		public override ETileset Tileset { get { return ETileset.CROSSBOW; } }

		public override string Name { get { return "арбалет"; } }
	}
}