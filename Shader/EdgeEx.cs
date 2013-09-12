using System;
using System.Drawing;
using GameCore;
using GameCore.Mapping;

namespace Shader
{
    public class EdgeEx
    {
        public PointF P1;
        public PointF P2;
		public PointF CellCenter;

        public bool Valid;
        public float Distance;
        public float Opacity;

        public LiveMapCell LiveMapCell;

        public EdgeEx(PointF _p1, PointF _p2) 
        {
            Valid = true;
            P1 = _p1;
            P2 = _p2;
            Distance = 0;
            Opacity = 0;
            TransparentColor = FColor.Empty;
        }

        public Vector Vector
        {
            get { return new Vector(P1, P2); }
        }

        public FColor TransparentColor;

        public override string ToString()
        {
            return string.Format("{2}[{0};{1}]", P1, P2, Valid ? "+" : "-");
        }

        public static float Distant(PointF p1, PointF p2)
        {
            var d = new PointF(p1.X - p2.X, p1.Y - p2.Y);
            return (float)Math.Sqrt(d.X * d.X + d.Y * d.Y);
        }

        /// <summary>
        ///     Проверка ориентации отрезка относительно точки
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float Orient(PointF p)
        {
            return (P1.X - p.X) * (P2.Y - p.Y) - (P1.Y - p.Y) * (P2.X - p.X);
        }
    }
}