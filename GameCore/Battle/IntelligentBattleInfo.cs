using System;
using GameCore.Creatures;

namespace GameCore.Battle
{
	public class IntelligentBattleInfo:CreatureBattleInfo
	{
		private readonly Intelligent m_intelligent;

		public IntelligentBattleInfo(Intelligent _intelligent)
		{
			m_intelligent = _intelligent;
		}
	}
}