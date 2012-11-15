namespace GameCore.Objects.Ammo
{
	internal class StackOfCrossBowBolts : StackOfAmmo
	{
		public StackOfCrossBowBolts(Material _material) : base(_material) { }

		public override ETiles Tileset { get { return ETiles.CROSSBOW_BOLT; } }

		protected override string NameOfSingle { get { return "болт"; } }
	}
}