using GameCore.Misc;

namespace GameCore.PathFinding
{
	public struct PathFinderNode
	{
		#region Variables Declaration

		public int F;
		public int G;
		public int H; // f = gone + heuristic
		public int PX; // Parent
		public int PY;
		public int X;
		public int Y;

		#endregion

		public Point Point { get { return new Point(X, Y); } }

		public override string ToString() { return X + ";" + Y; }
	}
}