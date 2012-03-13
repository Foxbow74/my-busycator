using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameCore;
using GameUi;

namespace ResourceEditor
{
	public partial class TilesList : UserControl, ITilesList, IResouceEditor
	{
		private readonly ResourceProvider m_rp;
		private readonly ListBox m_lb;
		private readonly TexturePresenter m_tp;
		private readonly FlowLayoutPanel m_pnl;
		private readonly TileEditor m_te;
		private readonly Dictionary<string,Tile> m_customTiles=new Dictionary<string, Tile>();

		public TilesList(ResourceProvider _rp)
		{
			m_rp = _rp;

			InitializeComponent();

			SuspendLayout();

			m_te = new TileEditor(m_rp){Dock = DockStyle.Fill, Padding = new Padding(5), Visible = false};
			Controls.Add(m_te);

			m_pnl = new FlowLayoutPanel { Dock = DockStyle.Top, Padding = new Padding(5), BackColor = Color.Gray, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowOnly, };
			Controls.Add(m_pnl);

			var splh = new Splitter { Dock = DockStyle.Top, BackColor = Color.DarkGray};
			Controls.Add(splh);

			m_tp = new TexturePresenter(_rp, this){Dock = DockStyle.Top};
			Controls.Add(m_tp);

			var splv = new Splitter { Dock = DockStyle.Left };
			Controls.Add(splv);

			m_lb = new ListBox { Dock = DockStyle.Left, MinimumSize = new Size(150, 1) };
			Controls.Add(m_lb);

			foreach (var tile in TileHelper.AllTiles.Keys.OrderBy(_tiles => _tiles.ToString()))
			{
				m_lb.Items.Add(tile);
			}
			m_lb.SelectedIndexChanged += MLbSelectedIndexChanged;

			ResumeLayout(false);
			m_lb.SelectedIndex = 0;
		}

		void MLbSelectedIndexChanged(object _sender, EventArgs _e)
		{
			SuspendLayout();
			foreach (var presenter in m_pnl.Controls.OfType<TilePresenter>())
			{
				presenter.Click -=PresenterOnClick;
			}
			m_pnl.Controls.Clear();
			
			foreach (var tile in GetTiles(m_lb.SelectedItem))
			{
				AddTilePresenter(tile);
			}

			ResumeLayout(true);
			if (m_pnl.Controls.Count > 0)
			{
				PresenterOnClick(m_pnl.Controls[0], null);
			}
			else
			{
				m_te.Visible = false;
			}
		}

		private IEnumerable<Tile> GetTiles(object _item)
		{
			if(_item is string)
			{
				yield return m_customTiles[(string)_item];
				yield break;
			}
			var tile = TileHelper.AllTiles[(ETiles) _item];
			if (tile is Tile)
			{
				yield return (Tile)tile;
			}
		}

		public void AddTile(Tile _tile)
		{
			PresenterOnClick(AddTilePresenter(_tile), null);
		}

		public void SetTile(ETextureSet _selectedItem, int _x, int _y)
		{
			var presenter = m_pnl.Controls.OfType<TilePresenter>().SingleOrDefault(_presenter => _presenter.IsSelected);
			if (presenter==null) return;
			presenter.Set(_selectedItem, _x, _y);
			PresenterOnClick(presenter,EventArgs.Empty);
		}

		private TilePresenter AddTilePresenter(Tile _tl)
		{
			var tilePresenter = new TilePresenter(_tl, m_rp);
			tilePresenter.Click += PresenterOnClick;
			m_pnl.Controls.Add(tilePresenter);
			return tilePresenter;
		}

		private void PresenterOnClick(object _sender, EventArgs _eventArgs)
		{
			foreach (var presenter in m_pnl.Controls.OfType<TilePresenter>())
			{
				presenter.IsSelected = presenter==_sender;
			}

			m_te.Init(((TilePresenter)_sender).Tile, (TilePresenter)_sender);
			m_te.Visible = true;
			m_tp.SelectTile(((TilePresenter)_sender).Tile);
		}

		public void SaveResources()
		{
			var sb = new StringBuilder();
			foreach (var item in m_lb.Items)
			{
				foreach (var tile in GetTiles(item))
				{
					sb.AppendLine(tile.ToText());
				}
			}
			File.WriteAllText(@"Resources\tiles.dat", sb.ToString());
			File.Copy(@"Resources\tiles.dat", @"..\ResourceEditor\Resources\tiles.dat", true);
		}

		public void AddTile(string _text)
		{
			if (!m_lb.Items.Contains(_text))
			{
				m_lb.Items.Add(_text);
				m_customTiles.Add(_text, new Tile(ETextureSet.RJ, 0,0,FColor.White));
			}
			m_lb.SelectedItem = _text;
		}
	}
}
