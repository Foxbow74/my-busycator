using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnsafeUtils;

namespace GameCore.Misc
{
	public class Profiler : IDisposable
	{
		private static readonly HiResTimer m_hrt = new HiResTimer();
		private static readonly Dictionary<string, Info> m_infos = new Dictionary<string, Info>();

		public static readonly Int64 Start = m_hrt.Value;
		readonly static List<Profiler> m_profilers = new List<Profiler>();
		private readonly Info m_info;
		private readonly string m_key;
		private readonly string m_name;
		private readonly Int64 m_time = m_hrt.Value;

		public Profiler()
		{
			var line = Environment.StackTrace.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)[3];
			m_name = line.Substring(0, line.IndexOf(')') + 1);
			m_key = string.Join(".", m_profilers.Select(_profiler => _profiler.m_name))  + "." + m_name;

			if (!m_infos.TryGetValue(m_key, out m_info))
			{
				m_infos[m_key] = m_info = new Info { Span = TimeSpan.Zero, In = m_profilers.Count == 0 ? null : m_profilers.Last().m_name, Name = m_name.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last(), Key = m_name };
			}
			m_profilers.Add(this);
		}

		public Profiler(string _name)
		{
			m_name = _name;
			m_key = string.Join(".", m_profilers.Select(_profiler => _profiler.m_name)) + "." + m_name;

			if (!m_infos.TryGetValue(m_key, out m_info))
			{
				m_infos[m_key] = m_info = new Info { Span = TimeSpan.Zero, In = m_profilers.Count == 0 ? null : m_profilers.Last().m_name, Name = m_name, Key = m_name };
			}

			m_profilers.Add(this);
		}

		#region IDisposable Members

		public void Dispose()
		{
			m_profilers.Remove(this);

			float prev = 0;
			if (m_info.Count > 1000)
			{
				prev = (float) m_info.Span.Ticks/m_info.Count;
			}
			m_info.Span += new TimeSpan(m_hrt.Value - m_time);
			m_info.Count++;

			if (prev > 0)
			{
				var now = m_info.Span.Ticks/m_info.Count;
				if (now/prev > 1.1)
				{
					Debug.WriteLine(m_name + " >>> " + now/prev*100 + "%");
				}
			}
		}

		#endregion

		private static void ReportIt(string _name, long _total, int _indent = 0)
		{
			var _spanSum = m_infos.Values.Where(_info => _info.In == _name).Aggregate(TimeSpan.Zero, (_current, _info) => _current + _info.Span).Ticks;
			var mains = m_infos.Values.Where(_info => _info.In == _name).OrderByDescending(_info => _info.Span);
			foreach (var info in mains)
			{
				var percents = 100*info.Span.Ticks/_total;
				if(percents==0)
				{
					continue;
				}
				Debug.WriteLine(new string('\t', _indent) + string.Format("***\t{0}\ttakes\t{1:N0}% ({2} sec)\tcalled\t{3}\t({4:N0}% in total)", info.Name, 100 * info.Span.Ticks / _spanSum, info.Span.TotalSeconds, info.Count, percents));
				ReportIt(info.Key, _total, _indent + 1);
			}
		}

		public static void Report()
		{
			var total = m_hrt.Value - Start;
			ReportIt(null, total);
		}

		#region Nested type: Info

		private class Info
		{
			public int Count;
			public String In;

			public String Key;
			public String Name;
			public TimeSpan Span;
		}

		#endregion
	}
}