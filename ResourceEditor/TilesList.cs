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

			var cm = new ContextMenu();
			cm.Popup += (_sender, _args) => SelectItemUnderMouse();
			cm.MenuItems.Add(new MenuItem("Переименовать", RenameItem) { Shortcut = Shortcut.F2, ShowShortcut = true });
			cm.MenuItems.Add(new MenuItem("Удалить", DeleteItem) { Shortcut = Shortcut.Del, ShowShortcut = true });

			m_lb = new ListBox { Dock = DockStyle.Left, MinimumSize = new Size(150, 1), Sorted = true, ContextMenu = cm};
			Controls.Add(m_lb);

			foreach (ETiles tile in Enum.GetValues(typeof(ETiles)))
			{
				m_lb.Items.Add(tile);
			}
			m_lb.SelectedIndexChanged += MLbSelectedIndexChanged;

			ResumeLayout(false);
			m_lb.SelectedIndex = 0;
		}

		#region context menu

		private void RenameItem(object _sender, EventArgs _eventArgs)
		{
			var selectedItem = m_lb.SelectedItem;
			var selectedIndex = m_lb.SelectedIndex;
			var ib = new InputBox() { String = selectedItem.ToString() };
			if (ib.ShowDialog() != DialogResult.OK) return;
			RemoveItem(selectedIndex);
			m_customTiles.Add(ib.String.ToUpper(), GetTile(selectedItem));
			m_lb.Items.Add(ib.String);
			m_lb.SelectedItem = ib.String;
		}

		private void DeleteItem(object _sender, EventArgs _eventArgs)
		{
			var selectedItem = m_lb.SelectedItem;
			var selectedIndex = m_lb.SelectedIndex;
			var ib = new InputBox() { String = "Удалить " + selectedItem + "?" };
			if (ib.ShowDialog() == DialogResult.OK)
			{
				RemoveItem(selectedIndex);
			}
		}

		private void RemoveItem(int _selectedIndex)
		{
			m_lb.SelectedIndex = _selectedIndex > 0 ? _selectedIndex - 1 : _selectedIndex + 1;
			m_lb.Items.RemoveAt(_selectedIndex);
		}


		private void SelectItemUnderMouse()
		{
			var p = m_lb.PointToClient(MousePosition);

			p = new Point(p.X, p.Y);


			var index = m_lb.IndexFromPoint(p);
			if (index > -1)
			{
				m_lb.SelectedIndex = index;
			}
		}

		#endregion


		void MLbSelectedIndexChanged(object _sender, EventArgs _e)
		{
			SuspendLayout();
			foreach (var presenter in m_pnl.Controls.OfType<TilePresenter>())
			{
				presenter.Click -=PresenterOnClick;
			}
			m_pnl.Controls.Clear();

			AddTilePresenter(GetTile(m_lb.SelectedItem));

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

		private Tile GetTile(object _item)
		{
			if(_item is string)
			{
				return m_customTiles[(string)_item];
			}
			if (!TileHelper.AllTiles.ContainsKey((ETiles)_item))
			{
				TileHelper.AllTiles.Add((ETiles)_item, new Tile(ETextureSet.RJ, 0, 0, FColor.White));
			}
			return (Tile)TileHelper.AllTiles[(ETiles)_item];
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
				sb.AppendLine(item + GetTile(item).ToShortText());
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
