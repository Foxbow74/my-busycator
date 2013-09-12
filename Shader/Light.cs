using System.Drawing;
using GameCore.Mapping;
using GameCore.Misc;

namespace Shader
{
    struct Light
    {
        public PointF Point;
        public ILightSource LightSource;
        public LiveMapCell LiveMapCell;

        public override string ToString()
        {
            return LightSource==null?"":LightSource.ToString();
        }
    }
}