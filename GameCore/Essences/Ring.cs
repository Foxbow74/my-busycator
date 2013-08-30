using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences
{
	public class Ring : Item
	{
		public Ring(Material _material) : base(EALNouns.Ring, _material) { }

        public override int TileIndex
        {
            get
            {
                return 1;
            }
        }

		public override EItemCategory Category { get { return EItemCategory.RINGS; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return ItemBattleInfo.Empty;
		}
	}
}