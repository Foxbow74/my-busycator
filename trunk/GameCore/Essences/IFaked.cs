using GameCore.Creatures;

namespace GameCore.Essences
{
	public interface IFaked : ISpecial
	{
		Essence ResolveFake(Creature _creature);

        ETileset Tileset { get; }

        int TileIndex { get; }
	}
}