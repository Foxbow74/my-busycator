using GameCore;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons
{
	public class Axe : AbstractMeleeWeapon
	{
		public Axe(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override string Name { get { return "топор"; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,0,new Dice(2,2,0));
		}
	}
}