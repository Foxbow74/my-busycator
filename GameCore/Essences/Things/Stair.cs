using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping.Layers;

namespace GameCore.Essences.Things
{
	public abstract class Stair : Thing
	{
		private WorldLayer m_leadToLayer;

		protected Stair(WorldLayer _leadToLayer, Material _material) : base(_material)
		{
			m_leadToLayer = _leadToLayer;
			Sex = ESex.FEMALE;
		}

		protected Stair(Material _material) : base(_material) { }

		public override EMaterialType AllowedMaterialsType
		{
			get { return EMaterialType.MINERAL; }
		}

		public EActResults MoveToLayer(Creature _creature)
		{
			if (m_leadToLayer == null)
			{
				m_leadToLayer = World.TheWorld.GenerateNewLayer(_creature, this);
			}
			World.TheWorld.CreatureManager.MoveCreature(_creature, _creature.GeoInfo.WorldCoords, m_leadToLayer);

			return EActResults.DONE;
		}
	}
}