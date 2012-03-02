using GameCore.Creatures;

namespace GameCore.Objects.Furniture
{
	public class Sign: FurnitureThing, ISpecial
	{
		private readonly ETiles m_tile;
		private readonly FColor m_lerpColor;
		private readonly string m_name;

		public Sign(ETiles _tile, FColor _lerpColor, string _name)
		{
			m_tile = _tile;
			m_lerpColor = _lerpColor;
			m_name = _name;
		}

		public override ETiles Tile
		{
			get { return m_tile; }
		}

		public override FColor LerpColor
		{
			get
			{
				return m_lerpColor;
			}
		}

		public override string Name
		{
			get { return m_name; }
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}