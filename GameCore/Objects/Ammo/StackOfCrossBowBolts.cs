namespace GameCore.Objects.Ammo
{
	internal class StackOfCrossBowBolts : StackOfAmmo
	{
		public StackOfCrossBowBolts(Material _material) : base(_material) { }

		public override ETileset Tileset { get { return ETileset.CROSSBOW_BOLT; } }

		protected override string NameOfSingle { get { return "болт"; } }
	}
}