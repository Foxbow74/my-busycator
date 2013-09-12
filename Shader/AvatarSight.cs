using System;
using GameCore;
using GameCore.Mapping;
using GameCore.Misc;

namespace Shader
{
    class AvatarSight : ILightSource
    {
        public int Radius { get { return LosManagerEx.MAP_SIZE/2; } }

        public void LightCells(LiveMap _liveMap, Point _point)
        {
            throw new NotImplementedException();
        }

        public FColor Color { get { return FColor.White; } }
    }
}