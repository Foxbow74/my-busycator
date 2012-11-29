using GameCore;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Weapons;

namespace MagickSetting.Items.Weapons
{
	public class Axe : AbstractMeleeWeapon, ISpecial
	{
		private readonly string m_name;

		public Axe(Material _material, string _name) : base(_material)
		{
			m_name = _name;
		}

		public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override string Name { get { return m_name; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,0,0,new Dice(2,2,0));
		}
	}
}