using System;
using System.Linq;
using GameCore.Misc;

namespace GameCore.Mapping.Layers.SurfaceObjects
{
    public abstract class City
    {
        protected City(WorldLayer _layer, params Point[] _cityBlockIds)
        {
            Layer = _layer;
            CityBlockIds = _cityBlockIds.ToArray();
        }


        public WorldLayer Layer { get; private set; }

        public Point[] CityBlockIds { get; private set; }

        public abstract void GenerateCityBlock(MapBlock _block, Random _rnd, WorldLayer _worldLayer);
    }
}