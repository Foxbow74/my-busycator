using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons.NaturalWeapon
{
	public class Jaws : AbstractNaturalWeapon
	{
		public Jaws(ItemBattleInfo _info) : base("зубы".AsNoun(ESex.PLURAL, false), _info)
		{
			Sex = ESex.PLURAL;
		}
	}
}