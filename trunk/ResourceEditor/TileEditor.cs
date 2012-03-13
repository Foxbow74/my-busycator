using System;
using System.Drawing;
using System.Windows.Forms;
using GameCore;
using GameUi;

namespace ResourceEditor
{
	public partial class TileEditor : UserControl
	{
		private readonly ResourceProvider m_rp;

		private readonly TextBox m_tb;
		private Tile m_tile;

		private readonly ColorDialog m_cd;
		private readonly Button m_btn;

		private readonly PictureBox m_pnlSample;

		private TilePresenter m_presenter;


		public TileEditor(ResourceProvider _rp)
		{
			m_rp = _rp;
			InitializeComponent();
			SuspendLayout();
			DoubleBuffered = true;

			var pnl = new Panel() { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(5)};
			m_tb = new TextBox() { Dock = DockStyle.Top };
			Controls.Add(pnl);
			Controls.Add(m_tb);

			m_btn = new Button(){Text = @"Цвет", Dock = DockStyle.Top, Margin = new Padding(5,0,5,5)};

			m_pnlSample = new PictureBox() { Dock = DockStyle.Top, MinimumSize = new Size(1, ATile.Size * 4), BackgroundImageLayout = ImageLayout.Tile, BackColor = Color.Black, };

			m_btn.Click += BtnClick;
			pnl.SuspendLayout();
			pnl.Controls.AddRange(new Control[] { m_pnlSample, m_btn });
			pnl.ResumeLayout(false);
			ResumeLayout(false);

			m_cd = new ColorDialog(){FullOpen = true};
		}

		void BtnClick(object _sender, EventArgs _e)
		{
			m_cd.Color = Color.FromArgb((int)(m_tile.Color.R * 255), (int)(m_tile.Color.G*255), (int)(m_tile.Color.B * 255));
			if(m_cd.ShowDialog(this)==DialogResult.OK)
			{
				m_tile.Color = new FColor(1f, m_cd.Color.R / 255f, m_cd.Color.G / 255f, m_cd.Color.B / 255f);
				m_presenter.Rebuild();
				m_pnlSample.BackgroundImage = m_presenter.CurrentImage;
			}
		}

		public void Init(Tile _tile, TilePresenter _presenter)
		{
			m_tile = _tile;
			m_presenter = _presenter;
			m_tb.Text = _tile.ToText();
			m_pnlSample.BackgroundImage = _presenter.CurrentImage;
		}
	}
}
