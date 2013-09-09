using System;
using System.Drawing;
using GameCore.Essences.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shader;

namespace Tests
{
    [TestClass]
    public class EdgeTest : AbstractTest
    {
        [TestMethod]
        public void EdgeOrientPointInside()
        {
            var e1 = new EdgeEx(new PointF(0, 0), new PointF(2, 0));
            var e2 = new EdgeEx(new PointF(2, 0), new PointF(0, 2));
            var e3 = new EdgeEx(new PointF(0, 2), new PointF(0, 0));

            var p = new PointF(0.5f, 0.5f);

            Assert.IsTrue(e1.Orient(p) > 0);
            Assert.IsTrue(e2.Orient(p) > 0);
            Assert.IsTrue(e3.Orient(p) > 0);
        }

        [TestMethod]
        public void EdgeOrientPointOnFringe()
        {
            var e1 = new EdgeEx(new PointF(0, 0), new PointF(2, 0));
            var e2 = new EdgeEx(new PointF(2, 0), new PointF(0, 2));
            var e3 = new EdgeEx(new PointF(0, 2), new PointF(0, 0));

            Assert.IsTrue(Math.Abs(e1.Orient(new PointF(1,0))) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e3.Orient(new PointF(0,1))) < float.Epsilon);
        }

        [TestMethod]
        public void EdgeOrientPointOutside()
        {
            var e1 = new EdgeEx(new PointF(0, 0), new PointF(2, 0));
            var e2 = new EdgeEx(new PointF(2, 0), new PointF(0, 2));
            var e3 = new EdgeEx(new PointF(0, 2), new PointF(0, 0));

            Assert.IsTrue(e1.Orient(new PointF(1,-1)) < 0);
            Assert.IsTrue(e2.Orient(new PointF(3,3)) < 0);
            Assert.IsTrue(e3.Orient(new PointF(-1,1)) < 0);
        }
    }
}