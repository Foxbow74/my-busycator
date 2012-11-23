using GameCore.Battle;
using GameCore.Essences;
using GameCore.Mapping.Layers;

namespace GameCore.Creatures.Dummies
{
	public abstract class AbstractDummyCreature:Creature,ISpecial
	{
		protected AbstractDummyCreature(WorldLayer _layer, int _speed) : base(_layer, _speed)
		{
		}

		internal override CreatureBattleInfo CreateBattleInfo()
		{
			throw new System.NotImplementedException();
		}

		public override EFraction Fraction
		{
			get { return EFraction.DUMMY; }
		}

	}
}