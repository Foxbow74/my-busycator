using System.Collections.Generic;
using GameCore;

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
		void Clear(FColor _color);
		void Exit();
		IEnumerable<IAbstractLanguageProcessor> GetLanguageProcessors();
	}
}