using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	internal class Bed : Thing
	{
		public Bed(Material _material)
			: base("кровать".AsNoun(ESex.FEMALE, false), _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 0; } }
	}
}