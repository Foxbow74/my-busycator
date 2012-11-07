using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XColorVM:XObjectVM
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.COLOR;}
		}

		[X("A")]private IXValue<float> m_a;
		[X("R")]private IXValue<float> m_r;
		[X("G")]private IXValue<float> m_g;
		[X("B")]private IXValue<float> m_b;


		public float A { get { return m_a.Value; } set { m_a.Value = value; } }
		public float R { get { return m_r.Value; } set { m_r.Value = value; } }
		public float G { get { return m_g.Value; } set { m_g.Value = value; } }
		public float B { get { return m_b.Value; } set { m_b.Value = value; } }

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_a, () => A);
			BindProperty(m_r, () => R);
			BindProperty(m_g, () => G);
			BindProperty(m_b, () => B);
		}
	}

	static class XColorHelper
	{
		public static FColor GetFColor(this XColorVM _xc)
		{
			if (_xc == null) return FColor.Empty;
			return new FColor(_xc.A, _xc.R, _xc.G, _xc.B);
		}

		public static XColorVM GetXColorVM(this FColor _fc)
		{
			return new XColorVM { A = _fc.A, R = _fc.R, G = _fc.G, B = _fc.B };
		}

		public static System.Windows.Media.Color GetColor(this FColor _fc)
		{
			return System.Windows.Media.Color.FromArgb((byte)(_fc.A * 255), (byte)(_fc.R * 255), (byte)(_fc.G * 255), (byte)(_fc.B * 255));
		}

		public static System.Drawing.Color GetDColor(this FColor _fc)
		{
			return System.Drawing.Color.FromArgb((byte)(_fc.A * 255), (byte)(_fc.R * 255), (byte)(_fc.G * 255), (byte)(_fc.B * 255));
		}

		public static FColor GetFColor(this System.Drawing.Color _c)
		{
			return new FColor(_c.A / 255f, _c.R / 255f, _c.G / 255f, _c.B / 255f);
		}
	}
}
