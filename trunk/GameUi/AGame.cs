using System.Drawing;

namespace GameUi
{
	public interface IGameProvider
	{
		IDrawHelper DrawHelper { get; }
		int WidthInCells { get; }
		int HeightInCells { get; }
		int Width { get; }
		int Height { get; }
		bool IsActive { get; }
		IResourceProvider ResourceProvider { get; }
		KeyState KeyState { get; }
		void Clear(Color _color);
		void Exit();
		void DrawTextLayer();
	}
}