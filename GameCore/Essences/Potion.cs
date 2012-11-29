using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences
{
	public class Potion : Item
	{
		public Potion(Material _material) : base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

		public override string Name { get { return "булька"; } }

		public override EItemCategory Category { get { return EItemCategory.POTION; } }

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.MINERAL; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return ItemBattleInfo.Empty;
		}

		public void Drinked(Creature _creature)
		{
			//throw new NotImplementedException();
		}

		public bool IsAllowToDrink(Creature _creature) { return true; }
	}
}