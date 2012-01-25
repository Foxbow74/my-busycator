#region

using GameCore.Acts;
using GameCore.Misc;

#endregion

namespace GameCore.Creatures
{
	public class Monster : Creature
	{
		private static int m_n;

		internal Monster(Point _coords) : base(_coords, 100)
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

		public override void Thinking()
		{
			NextAct = new MoveAct(new Point(m_rnd.Next(3) - 1, m_rnd.Next(3) - 1));
		}
	}
}