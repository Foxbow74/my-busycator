using System;
using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Battle;
using GameCore.Mapping.Layers;
using GameCore.Essences;
using GameCore.Misc;
using RusLanguage;

namespace GameCore.Creatures
{
	public class Citizen : Intelligent, ISpecial
	{
		private readonly string m_name;
		private FColor m_lerpColor;

		public Citizen(Surface _layer, Random _rnd)
			: base(_layer, _rnd.Next(10) + 95, EIntellectGrades.INT) { m_name = _layer.GetNextCitizenName(Sex); }

		public override FColor LerpColor { get { return m_lerpColor; } }

		public override ETileset Tileset
		{
			get
			{
				return ETileset.CITIZEN;
			}
		}

		public override int TileIndex
		{
			get
			{
				return Nn;
			}
		}

		public override string IntelligentName { get { return m_name; } }

		public override void Resolve(Creature _creature) { }

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

			var build = ((Surface) Layer).City.Buildings.ToArray().RandomItem(World.Rnd);

			#endregion

			#region выбираем перву незанятую точку на внутреннем "порожке" здания

			foreach (var inDoorWorldCoord in build.InDoorWorldCoords)
			{
				var destLiveCell = this[inDoorWorldCoord - currentLiveCell.WorldCoords];
				if (destLiveCell.GetIsPassableBy(this, true) > 0)
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

		internal override CreatureBattleInfo CreateBattleInfo()
		{
			return new IntelligentBattleInfo(this);
		}

		public override EFraction Fraction
		{
			get { return EFraction.AVATAR; }
		}

		public override string ToString()
		{
			var result = Name;
			for (var index = 1; index < Roles.ToArray().Length; index++)
			{
				var role = Roles.ToArray()[index];
				result += "/" + role[Sex];
			}
			return result;
		}

		public void SetLerpColor(FColor _fColor) { m_lerpColor = _fColor; }
	}
}