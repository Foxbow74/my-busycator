using System;
using GameCore.Acts;
using Graphics;

namespace GameCore.Creatures
{
	/// <summary>
	/// В отличие от аватара, остальные существа живут в блоках?
	/// </summary>
	public class Monster:Creature
	{
		internal Monster(World _world, Point _coords): base(_world, _coords, 80)
		{
		}

		public override Act GetNextAct()
		{
			return new MoveAct(new Point(0, -1));
		}

		public override Tile Tile
		{
			get { return Tiles.Monster; }
		}

		public override string Name
		{
			get { return "существо"; }
		}
	}
}
