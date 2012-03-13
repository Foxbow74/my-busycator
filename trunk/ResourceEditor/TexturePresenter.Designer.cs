using System.Windows.Forms;

namespace ResourceEditor
{
	partial class TexturePresenter
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_tsmiAdd = new System.Windows.Forms.MenuItem();
			this.m_tsmiSet = new System.Windows.Forms.MenuItem();
			this.m_cms = new System.Windows.Forms.ContextMenu();
			
			this.SuspendLayout();
	
			this.m_cms.Name = "m_cms";
			
			// 
			// toolStripMenuItem1
			// 
			this.m_tsmiAdd.Name = "toolStripMenuItem1";
			this.m_tsmiAdd.Text = "Добавить";
			this.m_tsmiAdd.Click += new System.EventHandler(this.TsmiAddClick);

			if (m_tilesList is TerrainsEditor)
			{
				m_cms.MenuItems.Add(m_tsmiAdd);
			}

			this.m_tsmiSet.Name = "toolStripMenuItem2";
			this.m_tsmiSet.Text = "Установить";
			this.m_tsmiSet.Click += new System.EventHandler(this.TsmiSetClick);

			m_cms.MenuItems.Add(m_tsmiSet);
			
			// 
			// TexturePresenter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "TexturePresenter";
			
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenu m_cms;
		private MenuItem m_tsmiAdd;
		private MenuItem m_tsmiSet;

	}
}
