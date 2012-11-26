using GameCore.Creatures;

namespace GameCore.Essences.Faked
{
	public interface IFaked : ISpecial
	{
		ETileset Tileset { get; }

        int TileIndex { get; }
		Essence ResolveFake(Creature _creature);
	}
}