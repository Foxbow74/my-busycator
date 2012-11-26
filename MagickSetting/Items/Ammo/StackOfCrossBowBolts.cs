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

		protected override string NameOfSingle { get { return "болт"; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,10,new Dice(2,2));
		}
	}
}