using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences
{
	internal class Ring : Item
	{
		public Ring(Material _material) : base("кольцо".AsNoun(ESex.IT, false), _material) { }

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