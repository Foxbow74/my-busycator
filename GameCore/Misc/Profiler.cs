using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameCore.Misc
{
	public class Profiler : IDisposable
	{
		private static readonly Dictionary<string, Info> m_infos = new Dictionary<string, Info>();

		public static readonly DateTime Start = DateTime.Now;
		private readonly Info m_info;
		private readonly string m_name;
		private readonly DateTime m_time = DateTime.Now;

		public Profiler()
		{
			var line = Environment.StackTrace.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)[3];
			m_name = line.Substring(0, line.IndexOf(')') + 1);

			if (!m_infos.TryGetValue(m_name, out m_info))
			{
				m_infos[m_name] = m_info = new Info {Span = TimeSpan.Zero};
			}
		}

		public Profiler(string _name)
		{
			m_name = _name;
			if (!m_infos.TryGetValue(m_name, out m_info))
			{
				m_infos[m_name] = m_info = new Info {Span = TimeSpan.Zero};
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			float prev = 0;
			if (m_info.Count > 1000)
			{
				prev = (float) m_info.Span.Ticks/m_info.Count;
			}
			m_info.Span += DateTime.Now - m_time;
			m_info.Count++;

			if (prev != 0)
			{
				var now = m_info.Span.Ticks/m_info.Count;
				if (now/prev > 1.1)
				{
					Debug.WriteLine(m_name + " >>> " + now/prev*100 + "%");
				}
			}
		}

		#endregion

		public static void Report()
		{
			var ordered = m_infos.OrderByDescending(_pair => _pair.Value.Span);
			var spanSum = m_infos.Values.Aggregate(TimeSpan.Zero, (_current, _info) => _current + _info.Span).Ticks;
			var total = (DateTime.Now - Start).Ticks;
			foreach (var pair in ordered)
			{
				Debug.WriteLine(string.Format("***\t{0}\ttakes\t{1:N0}% ({2} sec)\tcalled\t{3}\t({4:N0}% in total)", pair.Key, 100*pair.Value.Span.Ticks/spanSum, pair.Value.Span.TotalSeconds, pair.Value.Count, 100*pair.Value.Span.Ticks/total));
			}
		}

		#region Nested type: Info

		private class Info
		{
			public int Count;
			public TimeSpan Span;
		}

		#endregion
	}
}