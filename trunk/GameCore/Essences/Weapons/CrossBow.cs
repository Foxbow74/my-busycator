using GameCore.Battle;
using GameCore.Creatures;

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

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,0,new Dice(1,3,0));
		}
	}
}