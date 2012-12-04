using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	internal class Table : Thing
	{
		public Table(Material _material) : base("стол".AsNoun(ESex.MALE, false), _material) { }

        public override int TileIndex { get { return 12; } }
	}
}