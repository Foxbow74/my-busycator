using System.Windows.Forms;

namespace ResourceEditor
{
	public partial class InputBox : Form
	{
		public InputBox()
		{
			InitializeComponent();
		}

		public string String
		{
			get { return m_tb.Text; }
		}
	}
}
