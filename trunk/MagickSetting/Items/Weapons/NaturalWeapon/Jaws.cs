using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons.NaturalWeapon
{
	public class Jaws : AbstractNaturalWeapon
	{
		public Jaws(ItemBattleInfo _info) : base(EALNouns.Jaws, _info)
		{
			Sex = ESex.PLURAL;
		}

		public override EALVerbs Verb
		{
			get { return EALVerbs.JAWS_WEAPON_VERB; }
		}
	}
}