using System;
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

		public override bool IsCreature
		{
			get
			{
				return false;
			}
		}

		public override EFraction Fraction
		{
			get { return EFraction.DUMMY; }
		}

		public override CreatureBattleInfo CreateBattleInfo()
		{
			throw new NotImplementedException();
		}
	}
}