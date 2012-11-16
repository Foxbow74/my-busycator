using GameCore.Essences;

namespace GameCore.Effects
{
	internal class HealEffect : Effect
	{
		public HealEffect() : base("лечение") { }

		public override EEffect EffectType { get { return EEffect.HEAL | EEffect.SELF; } }

		public override bool Act(Essence _target) { return true; }
	}
}