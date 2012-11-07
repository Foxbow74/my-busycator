using GameCore.Storage;
using XTransport;

namespace GameCore.Storeable
{
	class XColor : XObject
	{
		public XColor(){}

		public override EStoreKind Kind
		{
			get { return EStoreKind.COLOR; }
		}

		[X("A")]private IXValue<float> m_a;
		[X("R")]private IXValue<float> m_r;
		[X("G")]private IXValue<float> m_g;
		[X("B")]private IXValue<float> m_b;

		public float A { get { return m_a.Value; } set { m_a.Value = value; } }
		public float R { get { return m_r.Value; } set { m_r.Value = value; } }
		public float G { get { return m_g.Value; } set { m_g.Value = value; } }
		public float B { get { return m_b.Value; } set { m_b.Value = value; } }
	}
	
	static class XColorHelper
	{
		public static FColor GetFColor(this XColor _xc)
		{
			if (_xc == null) return FColor.Empty;
			return new FColor(_xc.A, _xc.R, _xc.G, _xc.B);
		}

		public static XColor GetXColor(this FColor _fc)
		{
			return new XColor { A = _fc.A, R = _fc.R, G = _fc.G, B = _fc.B};
		}
	}
}