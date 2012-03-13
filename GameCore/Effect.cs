using System.Diagnostics;
using GameCore.Objects;

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
		public abstract EEffect EffectType{ get; }

		public abstract bool Act(Thing _target);
	}
}