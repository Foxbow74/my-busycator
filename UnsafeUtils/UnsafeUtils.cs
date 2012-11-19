using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UnsafeUtils
{
	public static class UnsafeUtils
	{
		#region Win32APIs
		[DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
		public unsafe static extern bool ZeroMemory(byte* _destination, int _length);
		#endregion

		public static unsafe void ClearPathFinderNodeFast(PathFinderNodeFast[] _array)
		{
			fixed (PathFinderNodeFast* pGrid = _array)
			{
				ZeroMemory((byte*)pGrid, sizeof(PathFinderNodeFast) * _array.Length);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PathFinderNodeFast
	{
		public int F; // f = gone + heuristic
		public int G;
		public ushort PX; // Parent
		public ushort PY;
		public byte Status;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FastPoint
	{
		public int X;
		public int Y;
	}

	public class FastPointTest
	{
		public FastPointTest()
		{
			var fp1 = new FastPoint() { X = 1, Y = -2 };
			var fp2 = new FastPoint() { X = 1, Y = 2 };

			unsafe
			{
				var p1 = (Int64*)&fp1;
				var p2 = (Int64*)&fp2;
				Debug.WriteLine(*p1 == *p2);
			}
			
		}
	}
}
