using System;
using System.Diagnostics;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Battle;
using GameCore.Essences;
using GameCore.Mapping.Layers;

namespace GameCore.Creatures
{
	public class Citizen : Intelligent, ISpecial
	{
		private readonly Noun m_name;
		private FColor m_lerpColor;

		public Citizen(Surface _layer, Random _rnd, ESex _sex)
			: base(EALNouns.Ctitzen, _layer, _rnd.Next(10) + 95, EIntellectGrades.INT)
		{
			m_name = _layer.GetNextCitizenName(_sex);
		}

		public override bool IsUnique
		{
			get
			{
				return true;
			}
		}

		public override Noun Name
		{
			get
			{
				return base.Name + m_name;
			}
		}

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

		public override EFraction Fraction
		{
			get { return EFraction.AVATAR; }
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

			var build = ((Surface) GeoInfo.Layer).City.Buildings.ToArray().RandomItem(World.Rnd);
			if (build == null)
			{
				AddActToPool(new WaitAct());
				return EThinkingResult.NORMAL;
			}


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
						AddActToPool(new MoveToAct(this, path),int.MaxValue);
						return EThinkingResult.NORMAL;
					}
				}
			}

			#endregion

			AddActToPool(new WaitAct());

			return EThinkingResult.NORMAL;
		}

		public override CreatureBattleInfo CreateBattleInfo()
		{
			return new IntelligentBattleInfo(this, 0, 0, new Dice(5, 8));
		}

		public override string ToString()
		{
			var result = Name.Text;
			for (var index = 1; index < Roles.ToArray().Length; index++)
			{
				var role = Roles.ToArray()[index];
				result += "/" + role.Name;
			}
			return result;
		}

		public void SetLerpColor(FColor _fColor) { m_lerpColor = _fColor; }
	}
}