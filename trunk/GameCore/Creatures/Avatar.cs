using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar() : base(Point.Zero, 100)
		{
			m_silence = false;
		}

		public override ETiles Tile
		{
			get { return ETiles.AVATAR; }
		}

		public override string Name
		{
			get { return "аватар"; }
		}

		public override void Resolve(Creature _creature)
		{
			throw new System.NotImplementedException();
		}
	}
}