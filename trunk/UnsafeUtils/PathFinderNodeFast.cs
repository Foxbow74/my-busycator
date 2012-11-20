using System.Runtime.InteropServices;

namespace UnsafeUtils
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PathFinderNodeFast
	{
		public int F; // f = gone + heuristic
		public int G;
		public ushort PX; // Parent
		public ushort PY;
		public byte Status;
	}
}