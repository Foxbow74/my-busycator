namespace GameCore.Essences.Weapons
{
	public class CrossBow : RangedWeapon
	{
		public CrossBow(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override string Name { get { return "арбалет"; } }
	}
}