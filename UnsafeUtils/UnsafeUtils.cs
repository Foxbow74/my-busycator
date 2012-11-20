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
}
