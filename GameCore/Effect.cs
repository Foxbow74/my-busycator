using System.Diagnostics;
using GameCore.Essences;

namespace GameCore
{
	public abstract class Effect
	{
		protected Effect(string _name)
		{
			Name = _name;
			Debug.WriteLine(_name + " " + GetHashCode());
		}

		public string Name { get; private set; }
		public abstract EEffect EffectType { get; }

		public abstract bool Act(Essence _target);
	}
}