using GameCore.Creatures;

namespace GameCore.Objects
{
	public interface IFaked : ISpecial
	{
		Thing ResolveFake(Creature _creature);
	}
}