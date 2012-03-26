using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	public class Sign : FurnitureThing, ISpecial
	{
		private readonly string m_name;
		private readonly ETiles m_tile;

		public Sign(ETiles _tile, Material _material, string _name) : base(_material)
		{
			m_tile = _tile;
			m_name = _name;
		}

		public override ETiles Tile { get { return m_tile; } }

		public override string Name { get { return m_name; } }

		public override void Resolve(Creature _creature) { }
	}
}