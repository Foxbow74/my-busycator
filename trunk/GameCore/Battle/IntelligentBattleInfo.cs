using GameCore.Creatures;

namespace GameCore.Battle
{
	public class IntelligentBattleInfo:CreatureBattleInfo
	{
		public IntelligentBattleInfo(Intelligent _intelligent, int _dv, int _pv, Dice _hp):base(_intelligent, _dv, _pv, _hp)
		{
			ToHitModifier = 5;
		}
	}
}