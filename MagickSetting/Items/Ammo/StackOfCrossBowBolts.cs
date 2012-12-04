using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Ammo
{
	internal class StackOfCrossBowBolts : StackOfAmmo
	{
		public StackOfCrossBowBolts(Material _material) : base("болт".AsNoun(ESex.MALE, false), _material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,10,new Dice(2,2));
		}
	}
}