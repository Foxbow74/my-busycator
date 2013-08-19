using System.Collections.Generic;

namespace GameCore.AbstractLanguage
{
	/// <summary>
	/// исходная форма - что сделал
	/// </summary>
	public class Verb
	{
		public string InProcess { get; private set; }
		public string Done { get; private set; }

		public Verb(string _inProcess, string _done)
		{
			InProcess = _inProcess;
			Done = _done;
			SameAs = new List<Verb> {this};
		}

		public List<Verb> SameAs { get; private set; }

		public static Verb operator +(Verb _a, Verb _b)
		{
			_a.SameAs.Add(_b);
			return _a;
		}

	}
}