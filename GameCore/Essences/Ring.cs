using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Rings
{
	internal class Ring : Item
	{
		public Ring(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 1;
            }
        }

		public override EItemCategory Category { get { return EItemCategory.RINGS; } }

		public override string Name { get { return "кольцо"; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return ItemBattleInfo.Empty;
		}
	}
}