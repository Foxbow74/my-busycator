using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	/// <summary>
	/// Табличка на здании, на стене
	/// </summary>
	public class Sign : FurnitureThing, ISpecial
	{
		private readonly string m_name;
		private readonly ETileset m_tileset;

		public Sign(ETileset _tileset, Material _material, string _name) : base(_material)
		{
			m_tileset = _tileset;
			m_name = _name;
		}

		public override ETileset Tileset { get { return m_tileset; } }

		public override string Name { get { return m_name; } }

		public override void Resolve(Creature _creature) { }
	}
}