﻿using System;
using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Mapping.Layers;
using GameCore.Objects;
using RusLanguage;

namespace GameCore.Creatures
{
	public class Citizen : Intelligent, ISpecial
	{
		private readonly string m_name;

		public Citizen(Surface _layer, Random _rnd)
			: base(_layer, _rnd.Next(10) + 95, EIntellectGrades.INT)
		{
			m_name = _layer.GetNextCitizenName(Sex);
		}

		public override ETiles Tile
		{
			get
			{
				switch (Sex)
				{
					case ESex.MALE:
						return Nn % 2 == 0 ? ETiles.CITIZEN_MALE : ETiles.CITIZEN_MALE2;
						break;
					case ESex.FEMALE:
						return Nn % 2 == 0 ? ETiles.CITIZEN_FEMALE : ETiles.CITIZEN_FEMALE2;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
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
			var currentLiveCell = this[0, 0];
			var currBuilding = currentLiveCell.InBuilding;

			if (currBuilding != null)
			{
				AddActToPool(new LeaveBuildingAct());
				return EThinkingResult.NORMAL;
			}

			#region выбираем случайное здание отличное от текущего

			var arr = ((Surface)Layer).City.Buildings.ToArray();
			var build = arr[World.Rnd.Next(arr.Length)];

			#endregion

			#region выбираем перву незанятую точку на внутреннем "порожке" здания

			foreach (var inDoorWorldCoord in build.InDoorWorldCoords)
			{
				var destLiveCell = this[inDoorWorldCoord - currentLiveCell.WorldCoords];
				if (destLiveCell.GetPfIsPassableBy(this) > 0)
				{
					var path = World.TheWorld.LiveMap.PathFinder.FindPath(this, destLiveCell.PathMapCoords);
					if (path != null)
					{
						//если точка достижима
						AddActToPool(new MoveToAct(this, path));
						return EThinkingResult.NORMAL;
					}
				}
			}

			#endregion


			AddActToPool(new WaitAct());

			return EThinkingResult.NORMAL;
		}

		public override string ToString()
		{
			var result = Name;
			for (int index = 1; index < Roles.ToArray().Length; index++)
			{
				var role = Roles.ToArray()[index];
				result += "/" + role[Sex];
			}
			return result;
		}
	}
}
