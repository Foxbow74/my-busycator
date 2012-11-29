using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences
{
	/// <summary>
	/// 	“о, что существо может вз€ть и положить
	/// </summary>
	public abstract class Item : Essence
	{
		protected Item(Material _material) : base(_material) { }

		public override ETileset Tileset
		{
			get { return ETileset.ITEMS; }
		}

		public override EMaterialType AllowedMaterialsType { get { return EMaterialType.METAL | EMaterialType.MINERAL | EMaterialType.WOOD; } }

		public abstract EItemCategory Category { get; }
		public abstract ItemBattleInfo CreateItemInfo(Creature _creature);
	}
}