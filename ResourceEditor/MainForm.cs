﻿using System;
using System.Windows.Forms;
using GameUi;

namespace ResourceEditor
{
	public partial class MainForm : Form
	{
		private readonly TabControl m_tc;

		private readonly ResourceProvider m_rp = new ResourceProvider();

		public MainForm()
		{
			TileHelper.Init(m_rp, null);
			m_rp.FillTiles();

			SuspendLayout();
			m_tc = new TabControl { Dock = DockStyle.Fill, Appearance = TabAppearance.Normal, };

			var tp = new TabPage("Tiles");
			tp.Controls.Add(new TilesList(m_rp) { Dock = DockStyle.Fill });
			m_tc.TabPages.Add(tp);

			var tp1 = new TabPage("Terrains");
			tp1.Controls.Add(new TerrainsEditor(m_rp) { Dock = DockStyle.Fill });
			m_tc.TabPages.Add(tp1);


			Controls.Add(m_tc);
			ResumeLayout(false);
			InitializeComponent();
		}

		private void m_miSave_Click(object sender, System.EventArgs e)
		{
			foreach (TabPage tabPage in m_tc.TabPages)
			{
				foreach (var control in tabPage.Controls)
				{
					if (control is IResouceEditor)
					{
						((IResouceEditor)control).SaveResources();
					}
				}
			}
		}

		private void MTsmiAddTileClick(object _sender, EventArgs _e)
		{
			var control = m_tc.SelectedTab.Controls[0];
			if (control is TilesList)
			{
				var ib = new InputBox();
				if (ib.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(ib.String))
				{
					((TilesList)control).AddTile(ib.String.ToUpper());
				}
			}
			else if (control is TerrainsEditor)
			{
				var ib = new InputBox();
				if (ib.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(ib.String))
				{
					((TerrainsEditor)control).AddTerrain(ib.Text.ToUpper());
				}
			}
		}
	}

	internal interface IResouceEditor
	{
		void SaveResources();
	}
}