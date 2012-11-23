using GameCore.Battle;
using GameCore.Creatures;

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
		
		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return ItemBattleInfo.Empty;
		}

		protected override string NameOfSingle { get { return "болт"; } }
	}
}