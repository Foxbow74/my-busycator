using System;
using System.Collections.Generic;
using System.Drawing;
using GameCore;

namespace GameUi
{
	public interface IGameProvider
	{
		IDrawHelper DrawHelper { get; }
		void Clear(Color _color);
		void Exit();
		int WidthInCells{ get; }
		int HeightInCells { get; }
		int Width { get; }
		int Height { get; }
		bool IsActive { get; }
		IResourceProvider ResourceProvider { get; }
		KeyState KeyState { get; }
		void DrawTextLayer();
	}

	public class KeyState
	{
		public KeyState()
		{
			PressedKeys = new List<ConsoleKey>();
		}

		public List<ConsoleKey> PressedKeys { get; private set; }

		public EKeyModifiers KeyModifiers { get; set; }
	}
}