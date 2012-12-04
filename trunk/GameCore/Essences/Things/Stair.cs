using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping.Layers;

namespace GameCore.Essences.Things
{
	public abstract class Stair : Thing
	{
		private readonly Noun m_name;
		private WorldLayer m_leadToLayer;

		protected Stair(Noun _name, WorldLayer _leadToLayer, Material _material)
			: base(_name, _material)
		{
			m_name = _name;
			m_leadToLayer = _leadToLayer;
			Sex = ESex.FEMALE;
		}

		protected Stair(Material _material) : base("лестница".AsNoun(ESex.FEMALE, false), _material) { }

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

		public override Noun Name
		{
			get { return m_name; }
		}
	}
}