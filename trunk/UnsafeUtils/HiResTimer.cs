using System;
using System.Runtime.InteropServices;

namespace UnsafeUtils
{
	public class HiResTimer
	{
		private static readonly bool m_isPerfCounterSupported = false;
		private static readonly Int64 m_frequency = 0;

		[DllImport("kernel32.dll")]
		private static extern int QueryPerformanceCounter(ref Int64 _count);
		[DllImport("kernel32.dll")]
		private static extern int QueryPerformanceFrequency(ref Int64 _frequency);

		static HiResTimer()
		{
			var returnVal = QueryPerformanceFrequency(ref m_frequency);

			if (returnVal != 0 && m_frequency != 1000)
			{
				m_isPerfCounterSupported = true;
			}
			else
			{
				m_frequency = 1000;
			}
		}

		public Int64 Frequency
		{
			get
			{
				return m_frequency;
			}
		}

		public Int64 Value
		{
			get
			{
				Int64 tickCount = 0;

				if (m_isPerfCounterSupported)
				{
					QueryPerformanceCounter(ref tickCount);
					return tickCount;
				}
				else
				{
					return Environment.TickCount;
				}
			}
		}

		public Int64 GetMilliseconds(Int64 _counterAtStart)
		{
			return ((Value - _counterAtStart) * 10000) / Frequency;
		}
	}
}
