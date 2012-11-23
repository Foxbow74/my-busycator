using GameCore.Creatures;

namespace GameCore.Essences
{
	public interface IFaked : ISpecial
	{
		ETileset Tileset { get; }

        int TileIndex { get; }
		Essence ResolveFake(Creature _creature);
	}
}