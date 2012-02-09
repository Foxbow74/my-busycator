using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	internal abstract class Stair : Furniture
	{
		private WorldLayer m_leadToLayer;

		protected Stair(WorldLayer _leadToLayer)
		{
			m_leadToLayer = _leadToLayer;
		}

		protected Stair()
		{
		}

		public EActResults MoveToLayer(Creature _creature)
		{
			if(m_leadToLayer==null)
			{
				m_leadToLayer = World.TheWorld.GenerateNewLayer(_creature, this);
			}
			_creature.Layer = m_leadToLayer;
			return EActResults.DONE;
		}

		public override void Resolve(Creature _creature)
		{
		}
	}
}