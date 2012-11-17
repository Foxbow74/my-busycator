namespace GameCore.Essences.Ammo
{
	internal class StackOfCrossBowBolts : StackOfAmmo
	{
		public StackOfCrossBowBolts(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		protected override string NameOfSingle { get { return "болт"; } }
	}
}