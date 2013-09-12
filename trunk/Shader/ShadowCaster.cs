using GameCore.Mapping;

namespace Shader
{
    struct ShadowCaster
    {
        public float Opacity;
        public LiveMapCell LiveMapCell;

        public override string ToString()
        {
            return Opacity >0? "X" : "";
        }
    }
}