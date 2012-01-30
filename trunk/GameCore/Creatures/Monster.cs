using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Misc;

namespace GameCore.Creatures
{
	public class Monster : Creature
	{
		private static int m_n;

		public Monster(Point _coords)
			: base(_coords, 100)
		{
			NN = (m_n++).ToString();
		}

		public string NN { get; private set; }

		public override ETiles Tile
		{
			get { return ETiles.MONSTER; }
		}

		public override string Name
		{
			get { return "существо" + NN; }
		}

		public override void Resolve(Creature _creature)
		{
			
		}

		public override void Thinking()
		{
			AddActToPool(new MoveAct(), new Point(m_rnd.Next(3) - 1, m_rnd.Next(3) - 1));
		}
	}
}