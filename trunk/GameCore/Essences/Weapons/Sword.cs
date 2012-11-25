using GameCore.Battle;
using GameCore.Creatures;

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

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,1,4,new Dice(3,4,0));
		}
	}
}