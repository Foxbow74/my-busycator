using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using GameCore;
using GameCore.Misc;
using GameUi;

namespace ResourceEditor
{
	public partial class TilePresenter : UserControl
	{
		private readonly ResourceProvider m_rp;
		private readonly PictureBox m_pb;

		public TilePresenter(Tile _tile, ResourceProvider _rp)
		{
			Tile = _tile;
			m_rp = _rp;
			InitializeComponent();
			SuspendLayout();

			ClientSize = new Size(20, 20);

			m_pb = new PictureBox
			       	{
						BackgroundImageLayout = ImageLayout.Center,
						ClientSize = new Size(ATile.Size, ATile.Size),
						BackColor = Color.Black,
						Dock = DockStyle.Fill,
						Padding = new Padding(2)
			       	};

			Rebuild();			

			m_pb.Click+= (_sender, _args) => OnClick(_args);

			Controls.Add(m_pb);

			ResumeLayout(false);
		}

		public Tile Tile { get; private set; }

		public bool IsSelected
		{
			get
			{
				return BorderStyle == BorderStyle.Fixed3D;
			}
			set
			{
				BorderStyle = value?BorderStyle.Fixed3D : BorderStyle.None;
			}
		}

		public Image CurrentImage { get { return m_pb.BackgroundImage; } }

		public void Rebuild()
		{
			var bmp = new Bitmap(ATile.Size, ATile.Size, PixelFormat.Format32bppPArgb);
			var texture = m_rp.TextureSets[Tile.Set];
			var transparent = texture.GetPixel(0, 0);

			var rct = new Rct(0, 0, ATile.Size, ATile.Size);
			foreach (var point in rct.AllPoints)
			{
				var x = Tile.X * ATile.Size + point.X;
				var y = Tile.Y * ATile.Size + point.Y;

				var pixel = texture.GetPixel(x, y);
				if (pixel == transparent) continue;
				var fcolor = new FColor(pixel.A, pixel.R, pixel.G, pixel.B).Multiply(1f / 255f);
				var result = fcolor.Multiply(Tile.Color).Multiply(255);
				bmp.SetPixel(point.X, point.Y, Color.FromArgb((int)result.A, (int)result.R, (int)result.G, (int)result.B));
			}
			m_pb.BackgroundImage = bmp;
		}

		public void Set(ETextureSet _set, int _x, int _y)
		{
			Tile.Update(_set, _x, _y, Tile.Color);
			Rebuild();
		}
	}
}
