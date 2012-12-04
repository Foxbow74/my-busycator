using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons
{
	public class Axe : AbstractMeleeWeapon, ISpecial
	{
		public Axe(Noun _name, Material _material)
			: base(_name, _material)
		{
		}

		public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,0,new Dice(2,2,0));
		}
	}
}