using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons
{
	public class CrossBow : AbstractRangedWeapon
	{
		public CrossBow(Material _material) : base(EALNouns.Crossbow, _material) { }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,0,new Dice(1,3,0));
		}

		public override EALVerbs Verb
		{
			get { return EALVerbs.CLUB_WEAPON_VERB;}
		}
	}
}