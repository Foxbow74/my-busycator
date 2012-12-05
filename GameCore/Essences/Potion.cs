using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences
{
	public class Potion : Item
	{
		public Potion(Material _material) : base(EALNouns.Potion, _material) { Sex = ESex.IT; }

        public override int TileIndex
        {
            get
            {
                return 0;
            }
        }

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