using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GameCore;
using GameUi;

namespace ResourceEditor
{
	public partial class TexturePresenter : UserControl
	{
		private readonly ResourceProvider m_rp;
		private readonly ITilesList m_tilesList;
		private readonly ComboBox m_cb;
		private readonly PictureBox m_pb;
		private readonly Timer m_timer = new Timer();

		public TexturePresenter(ResourceProvider _rp, ITilesList _tilesList)
		{
			m_rp = _rp;
			m_tilesList = _tilesList;
			InitializeComponent();
			SuspendLayout();
			m_cb = new ComboBox(){Dock = DockStyle.Top, Margin = new Padding(0,0,0,5), DropDownStyle = ComboBoxStyle.DropDownList};
			m_cb.SelectedIndexChanged += CbSelectedIndexChanged;
			foreach (var set in m_rp.TextureSets.Keys)
			{
				m_cb.Items.Add(set);
			}

			m_pb = new PictureBox {Dock = DockStyle.Fill, Padding = new Padding(5), BackColor = Color.Magenta, BackgroundImageLayout = ImageLayout.None, ContextMenu = m_cms};

			m_pb.Paint += MPbPaint;
			m_pb.MouseMove += MPbMouseMove;
			m_pb.MouseUp += MPbMouseMove;

			m_timer.Interval = 100;
			m_timer.Tick += MTimerTick;

			Controls.Add(m_pb);
			Controls.Add(new Label {Size = new Size(5,5), Dock = DockStyle.Top});
			Controls.Add(m_cb);

			ResumeLayout(false);
			m_cb.SelectedIndex = 0;
			CurrentPoint = new Point();

			m_timer.Start();
		}

		void MPbMouseMove(object _sender, MouseEventArgs _e)
		{
			if (_e.Button == MouseButtons.Left)
			{
				var pnt = _e.Location;
				CurrentPoint = new Point(pnt.X/ATile.Size, pnt.Y/ATile.Size);
			}
		}

		void MTimerTick(object _sender, EventArgs _e)
		{
			m_pb.Invalidate();
		}

		private int m_tick = 0;

		void MPbPaint(object _sender, PaintEventArgs _e)
		{
			m_tick++;
			using (var pen = new Pen(Color.Yellow, 2f) { DashStyle = DashStyle.Dash, DashOffset = m_tick })
			{
				_e.Graphics.DrawRectangle(pen, CurrentPoint.X * ATile.Size, CurrentPoint.Y * ATile.Size, ATile.Size, ATile.Size);	
			}
		}
		
		public Point CurrentPoint { get; set; }

		void CbSelectedIndexChanged(object _sender, EventArgs _e)
		{
			m_pb.BackgroundImage = m_rp.TextureSets[(ETextureSet) m_cb.SelectedItem];
			m_pb.MinimumSize = m_pb.BackgroundImage.Size;
			MinimumSize = new Size(1, m_cb.Height + m_pb.BackgroundImage.Size.Height + 30);
		}

		public void SelectTile(Tile _tile)
		{
			m_cb.SelectedItem = _tile.Set;
			CurrentPoint = new Point(_tile.X, _tile.Y);
		}

		private void TsmiAddClick(object _sender, EventArgs _e)
		{
			m_tilesList.AddTile(new Tile((ETextureSet)m_cb.SelectedItem, CurrentPoint.X, CurrentPoint.Y, FColor.White));
		}

		private void TsmiSetClick(object _sender, EventArgs _e)
		{
			m_tilesList.SetTile((ETextureSet)m_cb.SelectedItem, CurrentPoint.X, CurrentPoint.Y);
		}
	}
}
