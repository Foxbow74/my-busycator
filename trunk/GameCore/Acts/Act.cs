#region

using GameCore.Creatures;

#endregion

namespace GameCore.Acts
{
	public abstract class Act
	{
		protected Act(int _takeTicks)
		{
			TakeTicks = _takeTicks;
		}

		public int TakeTicks { get; private set; }

		public abstract void Do(Creature _creature, bool _silence);
	}
}