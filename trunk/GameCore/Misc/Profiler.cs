using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace GameCore.Misc
{
	public class Profiler:IDisposable
	{
		class Info
		{
			public int Count;
			public TimeSpan Span;
		}

		static private readonly Dictionary<string, Info> m_infos = new Dictionary<string, Info>();
		private readonly DateTime m_time = DateTime.Now;
		private readonly string m_name;
		private readonly Info m_info;

		public Profiler()
		{
			var line = Environment.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[3];
			m_name = line.Substring(0, line.IndexOf(')')+1);

			if (!m_infos.TryGetValue(m_name, out m_info))
			{
				m_infos[m_name] = m_info = new Info { Span = TimeSpan.Zero };
			}
		}

		public Profiler(string _name)
		{
			m_name = _name;
			if (!m_infos.TryGetValue(m_name, out m_info))
			{
				m_infos[m_name] = m_info = new Info { Span = TimeSpan.Zero };
			}
		}

		public static void Report()
		{
			var ordered = m_infos.OrderByDescending(_pair => _pair.Value.Span);
			var spanSum = m_infos.Values.Aggregate(TimeSpan.Zero, (_current, _info) => _current + _info.Span).Ticks;
			Debug.WriteLine(spanSum);
			foreach (var pair in ordered)
			{
				Debug.WriteLine(string.Format("***\t{0}\ttakes\t{1:N2}% ({2})\tcalled\t{3}", pair.Key, 100 * pair.Value.Span.Ticks/spanSum, pair.Value.Span, pair.Value.Count));
			}
		}

		public void Dispose()
		{
			m_info.Span += DateTime.Now - m_time;
			m_info.Count++;
		}
	}
}
