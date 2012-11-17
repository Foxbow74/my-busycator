namespace GameCore.Essences
{
	/// <summary>
	/// 	“о, что существо может вз€ть и положить
	/// </summary>
	public abstract class Item : Essence
	{
        public override ETileset Tileset
        {
            get { return ETileset.ITEMS; }
        }

		protected Item(Material _material) : base(_material) { }

		public override EMaterial AllowedMaterials { get { return EMaterial.METAL | EMaterial.MINERAL | EMaterial.WOOD; } }
	}
}