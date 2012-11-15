using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping.Layers;
using RusLanguage;

namespace GameCore.Objects.Furnitures
{
	public abstract class Stair : Furniture
	{
		private WorldLayer m_leadToLayer;

		protected Stair(WorldLayer _leadToLayer, Material _material) : base(_material)
		{
			m_leadToLayer = _leadToLayer;
			Sex = ESex.FEMALE;
		}

		protected Stair(Material _material) : base(_material) { }

		public EActResults MoveToLayer(Creature _creature)
		{
			if (m_leadToLayer == null)
			{
				m_leadToLayer = World.TheWorld.GenerateNewLayer(_creature, this);
			}
			_creature.Layer = m_leadToLayer;

			return EActResults.DONE;
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.LANDSCAPE; }
		}

		public override void Resolve(Creature _creature) { }

		public override EMaterial AllowedMaterials
		{
			get { return EMaterial.MINERAL; }
		}
	}
}