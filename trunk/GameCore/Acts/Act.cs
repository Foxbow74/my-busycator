using GameCore.Creatures;

namespace GameCore.Acts
{
	public abstract class Act
	{
		public int TakeTicks { get; private set; }

		protected Act(int _takeTicks)
		{
			TakeTicks = _takeTicks;
		}

		public abstract void Do(Creature _creatures, World _world, bool _silence);
	}
}
