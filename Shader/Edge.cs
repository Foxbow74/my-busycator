using System;
using System.Drawing;
using GameCore;

namespace Shader
{
    public class Edge
    {
	    public bool Valid;
	    public float Distance;

	    public float Opacity;
        private PointF _p1;
        private PointF _p2;

        public Edge(PointF p1, PointF p2)
        {
	        Valid = true;
            _p1 = p1;
            _p2 = p2;
	        Distance = 0;
	        Opacity = 0;
	        TransparentColor = FColor.Empty;
        }

        public PointF AB
        {
            get
            {
                float A = (_p1.Y - _p2.Y)/(_p1.X - _p2.X);
                float B = _p1.Y - A*_p1.X;
                return new PointF(A, B);
            }
        }

        public PointF P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        public PointF P2
        {
            get { return _p2; }
            set { _p2 = value; }
        }

        public Vector Vector
        {
            get { return new Vector(_p1, _p2); }
        }

	    public FColor TransparentColor;

	    public override string ToString()
        {
            return string.Format("{2}[{0};{1}]", P1, P2, Valid ? "+" : "-");
        }

        public static float Distant(PointF p1, PointF p2)
        {
            var d = new PointF(p1.X - p2.X, p1.Y - p2.Y);
            return (float) Math.Sqrt(d.X*d.X + d.Y*d.Y);
        }

        public static PointF P2Vector(PointF p1, PointF p2)
        {
            return new PointF(p2.X - p1.X, p2.Y - p1.Y);
        }

        /// <summary>
        ///     Проверка ориентации отрезка относительно точки
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float Orient(PointF p)
        {
            return (_p1.X - p.X)*(_p2.Y - p.Y) - (_p1.Y - p.Y)*(_p2.X - p.X);
        }

        /// <summary>
        ///     проверка пересечения двух граней
        /// </summary>
        /// <param name="edg"></param>
        /// <returns></returns>
        public CrossResult Cross(Edge edg)
        {
            var result = new CrossResult();

            // знаменатель
            float Z = (P2.Y - P1.Y)*(edg.P1.X - edg.P2.X) - (edg.P1.Y - edg.P2.Y)*(P2.X - P1.X);
            // числитель 1
            float Ca = (P2.Y - P1.Y)*(edg.P1.X - P1.X) - (edg.P1.Y - P1.Y)*(P2.X - P1.X);
            // числитель 2
            float Cb = (edg.P1.Y - P1.Y)*(edg.P1.X - edg.P2.X) - (edg.P1.Y - edg.P2.Y)*(edg.P1.X - P1.X);

            // если знаменатель = 0, прямые параллельны
            if (Z == 0)
            {
                // если числители и знаменатель = 0, прямые совпадают
                if (Ca == 0 && Cb == 0)
                {
                    result.IsCross = false;
                    return result;
                }
                result.IsCross = false;
                return result;
            }

            float Ua = Ca/Z;
            float Ub = Cb/Z;


            if (Ua < 0 || Ua > 1 || Ub < 0 || Ub > 1)
            {
                //точка пересечения вне пределов отрезков
                result.IsCross = false;
                return result;
            }

            result.pt = new PointF(P1.X + (P2.X - P1.X)*Ub, P1.Y + (P2.Y - P1.Y)*Ub);

            return result;
        }

        public class CrossResult
        {
            public bool IsCross = true;
            public PointF pt;
        }
    }
}