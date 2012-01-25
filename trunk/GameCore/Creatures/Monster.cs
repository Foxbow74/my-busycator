using System;
using GameCore.Acts;
using GameCore.Misc;

namespace GameCore.Creatures
{
	/// <summary>
	/// В отличие от аватара, остальные существа живут в блоках?
	/// </summary>
	public class Monster:Creature
	{
		private static int n = 0;

		internal Monster(World _world, Point _coords): base(_world, _coords, 100)
		{
			NN = (n++).ToString();
		}

		public string NN { get; private set; }

		public override Act GetNextAct()
		{
			return new MoveAct(new Point(m_rnd.Next(3) - 1, m_rnd.Next(3)-1));
		}

		public override ETiles Tile
		{
			get { return ETiles.MONSTER; }
		}

		public override string Name
		{
			get { return "существо" + NN; }
		}
	}
}
