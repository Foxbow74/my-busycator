using GameCore;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons
{
	public class Sword : AbstractMeleeWeapon
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