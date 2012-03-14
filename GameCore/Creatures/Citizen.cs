using System;
using GameCore.Acts.Movement;
using GameCore.Mapping.Layers;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	class Citizen : Intelligent, ISpecial
	{
		private readonly string m_name;

		public Citizen(Surface _layer, Random _rnd)
			: base(_layer, _rnd.Next(10) + 95, EIntellectGrades.INT)
		{
			m_name = "Шмат" + GetHashCode();
		}

		public override ETiles Tile
		{
			get { return ETiles.CITIZEN; }
		}

		protected override string IntelligentName
		{
			get { return m_name; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		public override EThinkingResult Thinking()
		{
			AddActToPool(new MoveAct(), new Point(Rnd.Next(3) - 1, Rnd.Next(3) - 1));
			return EThinkingResult.NORMAL;
		}
	}
}
