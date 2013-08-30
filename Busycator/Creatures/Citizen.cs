using System;
using System.Linq;
using Busycator.Layers;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Mapping.Layers;

namespace Busycator.Creatures
{
    public class Citizen : Intelligent, ISpecial
    {
        private readonly Noun m_name;
        private FColor m_lerpColor;

        public Citizen(WorldLayer _layer, Random _rnd, Noun _name)
            : base(EALNouns.Ctitzen, _layer, _rnd.Next(10) + 95, EIntellectGrades.INT)
        {
            Sex = _name.Sex;
            m_name = _name;
        }

        protected override Noun GetUpdatedName(Noun _noun)
        {
            return _noun + m_name;
        }

        public override bool IsUnique
        {
            get
            {
                return true;
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

            var surface = GeoInfo.Layer as Surface;
            if (surface != null)
            {
                var build = surface.City.Buildings.ToArray().RandomItem(World.Rnd);
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
                            AddActToPool(new MoveToAct(this, path), int.MaxValue);
                            return EThinkingResult.NORMAL;
                        }
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
            var result = "";
            for (var index = 1; index < Roles.ToArray().Length; index++)
            {
                var role = Roles.ToArray()[index];
                result += (string.IsNullOrEmpty(result) ? ", " : "/") + role.Name;
            }
            return Name.AlsoKnownAs.Text + result;
        }

        public void SetLerpColor(FColor _fColor) { m_lerpColor = _fColor; }
    }
}