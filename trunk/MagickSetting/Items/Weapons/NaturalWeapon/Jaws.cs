using GameCore;
using GameCore.Battle;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons.NaturalWeapon
{
	public class Jaws : AbstractNaturalWeapon
	{
		public Jaws(ItemBattleInfo _info) : base(_info)
		{
			Sex = ESex.PLURAL;
		}

		public override string Name
		{
			get { return "зубы"; }
		}
	}
}