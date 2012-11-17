namespace GameCore.Essences.Weapons
{
	public class Sword : Weapon
	{
		public Sword(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 1;
            }
        }

		public override string Name { get { return "меч"; } }
	}
}