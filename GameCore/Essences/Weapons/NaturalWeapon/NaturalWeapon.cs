using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Weapons
{
	public abstract class NaturalWeapon : Weapon, ISpecial
	{
		private ItemBattleInfo m_info;

		protected NaturalWeapon(ItemBattleInfo _info) : base(null)
		{
			m_info = _info;
		}

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return m_info;
		}
	}

	public class Jaws:NaturalWeapon
	{
		public Jaws(ItemBattleInfo _info) : base(_info)
		{
		}

		public override string Name
		{
			get { return "зубы"; }
		}
	}
}