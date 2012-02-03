namespace GameCore.Objects.Ammo
{
	class StackOfCrossBowBolts:StackOfAmmo
	{
		public override ETiles Tile
		{
			get
			{
				return ETiles.CROSSBOW_BOLT;
			}
		}

		protected override string NameOfSingle
		{
			get { return "арбалетный болт"; }
		}
	}
}