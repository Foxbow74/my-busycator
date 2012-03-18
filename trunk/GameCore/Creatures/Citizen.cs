using System;
using System.Diagnostics;
using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Interact;
using GameCore.Acts.Movement;
using GameCore.Mapping.Layers;
using GameCore.Objects;

namespace GameCore.Creatures
{
	class Citizen : Intelligent, ISpecial
	{
		private readonly string m_name;

		public Citizen(Surface _layer, Random _rnd)
			: base(_layer, 50, EIntellectGrades.INT)// _rnd.Next(10) + 95
		{
			m_name = _layer.GetNextCitizenName(Sex);
		}

		public override ETiles Tile
		{
			get { return ETiles.CITIZEN; }
		}

		public override string IntelligentName
		{
			get { return m_name; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		public override EThinkingResult Thinking()
		{
			var building = this[0, 0].InBuilding;
			if(building!=null)
			{
				AddActToPool(new LeaveBuildingAct());
				return EThinkingResult.NORMAL;
			}
			AddActToPool(new WaitAct());

			return EThinkingResult.NORMAL;
		}

		public override string ToString()
		{
			string result = Name;
			foreach (var role in Roles)
			{
				result +=  "/" + role;
			}
			return result;
		}
	}
}
