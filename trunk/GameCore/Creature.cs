using GameCore.Objects;
using Graphics;

namespace GameCore
{
	public class Creature: Object
	{
		public Creature():base()
		{
			Point = new Point();
		}

		public Point Point { get; private set; }
	}
}