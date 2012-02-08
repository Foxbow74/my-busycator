using System.Drawing;
using GameCore.Objects;



namespace GameUi.UIBlocks.Items
{
	internal class ContainerPresenter : ILinePresenter
	{
		private readonly Container m_container;

		public ContainerPresenter(Container _container)
		{
			m_container = _container;
		}

		#region ILinePresenter Members

		public virtual void DrawLine(int _line,UIBlock _uiBlock)
		{
			var cntnr = m_container == null ? "на земле" : m_container.Name;
			_uiBlock.DrawLine("*** " + cntnr.ToUpper() + " ***", Color.White,_line, 0, EAlignment.CENTER);
		}

		#endregion
	}
}