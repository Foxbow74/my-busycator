using GameCore.Creatures;

namespace GameCore.Battle
{
	public class IntelligentBattleInfo:CreatureBattleInfo
	{
		public IntelligentBattleInfo(Intelligent _intelligent, int _dv, int _pv, Dice _hitDice):base(_intelligent, _dv, _pv, _hitDice)
		{
			ToHitModifier = 5;
		}
	}

	public class MonsterBattleInfo : CreatureBattleInfo
	{
		public MonsterBattleInfo(AbstractMonster _monster, int _dv, int _pv, Dice _hitDice)
			: base(_monster, _dv, _pv, _hitDice)
		{
			ToHitModifier = 5;
		}

		public AbstractMonster Monster { get { return (AbstractMonster)Creature; } }
	}
}