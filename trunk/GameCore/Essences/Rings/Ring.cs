using GameCore.Creatures;

namespace GameCore.Essences.Rings
{
	internal class Ring : Item
	{
		public Ring(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 1;
            }
        }

		public override EEssenceCategory Category { get { return EEssenceCategory.RINGS; } }

		public override string Name { get { return "кольцо"; } }

		public override void Resolve(Creature _creature) { }
	}
}