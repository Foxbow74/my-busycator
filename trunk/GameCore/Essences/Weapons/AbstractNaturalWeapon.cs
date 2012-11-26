using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Weapons
{
	public abstract class AbstractNaturalWeapon : AbstractWeapon, ISpecial
	{
		private readonly ItemBattleInfo m_info;

		protected AbstractNaturalWeapon(ItemBattleInfo _info) : base(null)
		{
			m_info = _info;
		}

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return m_info;
		}
	}
}