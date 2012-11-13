using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameCore;
using GameCore.Storage;
using ResourceWizard.Properties;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
    internal class XColorVM : XObjectVM
    {
        public XColorVM()
        {
            SelectColorCommand = new RelayCommand(ExecuteSelectColorCommand);
        }

        private void ExecuteSelectColorCommand(object _obj)
        {
            Manager.Instance.COLOR_DIALOG.Color = this.GetDColor();
            if (Manager.Instance.COLOR_DIALOG.ShowDialog() != DialogResult.Cancel)
            {
                Set(Manager.Instance.COLOR_DIALOG.Color.GetFColor());
            }
        }

        public override EStoreKind Kind
        {
            get { return EStoreKind.COLOR; }
        }

        [X("A")]
        private IXValue<float> m_a;
        [X("R")]
        private IXValue<float> m_r;
        [X("G")]
        private IXValue<float> m_g;
        [X("B")]
        private IXValue<float> m_b;

        public RelayCommand SelectColorCommand { get; private set; }

        public BitmapSource ColorsImage { get { return Resources.colors.Source(); } }
        public BitmapSource ColorsImageD { get { return Resources.colors.SourceDisabled(); } }

        public float A
        {
            get { return m_a.Value; }
            set { m_a.Value = value; }
        }

        public float R
        {
            get { return m_r.Value; }
            set { m_r.Value = value; }
        }

        public float G
        {
            get { return m_g.Value; }
            set { m_g.Value = value; }
        }

        public float B
        {
            get { return m_b.Value; }
            set { m_b.Value = value; }
        }

        protected override void InstantiationFinished()
        {
            base.InstantiationFinished();
            BindProps();
        }

        public void BindProps()
        {
            BindProperty(m_a, () => A);
            BindProperty(m_r, () => R);
            BindProperty(m_g, () => G);
            BindProperty(m_b, () => B);
        }

		public void Set(FColor _fColor)
		{
			R = _fColor.R;
			G = _fColor.G;
			B = _fColor.B;
			A = _fColor.A;
		}
	}

    static class XColorHelper
    {
        public static FColor GetFColor(this XColorVM _xc)
        {
            if (_xc == null) return FColor.White;
            return new FColor(_xc.A, _xc.R, _xc.G, _xc.B);
        }

		public static System.Drawing.Color GetDColor(this XColorVM _xc)
		{
			return System.Drawing.Color.FromArgb((byte)(_xc.A * 255), (byte)(_xc.R * 255), (byte)(_xc.G * 255), (byte)(_xc.B * 255));
		}

		public static System.Windows.Media.Color GetColor(this XColorVM _xc)
		{
		    return GetColor(_xc.GetFColor());
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
