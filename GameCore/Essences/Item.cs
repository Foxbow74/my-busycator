using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences
{
	/// <summary>
	/// 	��, ��� �������� ����� ����� � ��������
	/// </summary>
	public abstract class Item : Essence
	{
		protected Item(Material _material) : base(_material) { }

		public override ETileset Tileset
		{
			get { return ETileset.ITEMS; }
		}

		public override EMaterial AllowedMaterials { get { return EMaterial.METAL | EMaterial.MINERAL | EMaterial.WOOD; } }

		public abstract EItemCategory Category { get; }
		public abstract ItemBattleInfo CreateItemInfo(Creature _creature);
	}
}