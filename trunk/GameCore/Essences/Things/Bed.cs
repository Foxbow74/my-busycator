using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	internal class Bed : Thing
	{
		public Bed(Material _material)
			: base(EALNouns.Bed, _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 0; } }
	}
}