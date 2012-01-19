using System;
using System.Linq;
using System.Collections.Generic;
using GameCore;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class MainBlock:UIBlock
	{
		public World World { get; private set; }
		private readonly GraphicsDevice m_device;

		private readonly UIBlock m_map;
		private readonly UIBlock m_messages;
		private readonly UIBlock m_stats;
		private readonly MapCell[,] m_mapTiles;

		public MainBlock(GraphicsDevice _device, World _world)
			: base(new Rectangle(0, 0, _device.Viewport.Width / Tile.Size, _device.Viewport.Height / Tile.Size), null, Color.White)
		{
			World = _world;
			m_device = _device;
			var width = Rectangle.Width;
			var height = Rectangle.Height;

			var messagesHeight = 10;
			var statWidth = 15;

			m_stats = new UIBlock(new Rectangle(width - statWidth, 0, statWidth, height - messagesHeight + 1), Frame.SimpleFrame, Color.Gray);
			m_messages = new MessageBlock(new Rectangle(0, height - messagesHeight, width, messagesHeight));

			m_map = new UIBlock(new Rectangle(0, 0, m_stats.Rectangle.Left + 1, m_messages.Rectangle.Top + 1), Frame.SimpleFrame, Color.DarkRed);

			m_mapTiles = new MapCell[m_map.Rectangle.Width, m_map.Rectangle.Height];
		}

		public override void Draw(GameTime _gameTime, SpriteBatch _spriteBatch)
		{
			DrawBackground(_gameTime, _spriteBatch);
			DrawContent(_gameTime, _spriteBatch, World);
			DrawFrames(_spriteBatch);
		}

		private void DrawFrames(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			m_stats.PreDraw(_spriteBatch);
			m_messages.PreDraw(_spriteBatch);
			m_map.PreDraw(_spriteBatch);
			
			_spriteBatch.End();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_key==ConsoleKey.Q && _modifiers==EKeyModifiers.CTRL)
			{
				MessageManager.SendMessage(this, new OpenUIBlockMessage(new ConfirmQuitBlock()));
				return;
			}

			//var dx = (_pressedKeys.Contains(Keys.Left) ? -1 : 0) + (_pressedKeys.Contains(Keys.Right) ? 1 : 0);
			//var dy = (_pressedKeys.Contains(Keys.Up) ? -1 : 0) + (_pressedKeys.Contains(Keys.Down) ? 1 : 0);

			//dx += (_pressedKeys.Contains(Keys.NumPad4) ? -1 : 0) + (_pressedKeys.Contains(Keys.NumPad6) ? 1 : 0);

			//dx += (_pressedKeys.Contains(Keys.NumPad7) ? -1 : 0) + (_pressedKeys.Contains(Keys.NumPad9) ? 1 : 0);
			//dx += (_pressedKeys.Contains(Keys.NumPad1) ? -1 : 0) + (_pressedKeys.Contains(Keys.NumPad3) ? 1 : 0);

			//dx += (_pressedKeys.Contains(Keys.Home) ? -1 : 0) + (_pressedKeys.Contains(Keys.PageUp) ? 1 : 0);
			//dx += (_pressedKeys.Contains(Keys.End) ? -1 : 0) + (_pressedKeys.Contains(Keys.PageDown) ? 1 : 0);

			//dy += (_pressedKeys.Contains(Keys.NumPad8) ? -1 : 0) + (_pressedKeys.Contains(Keys.NumPad2) ? 1 : 0);

			//dy += (_pressedKeys.Contains(Keys.NumPad7) ? -1 : 0) + (_pressedKeys.Contains(Keys.NumPad1) ? 1 : 0);
			//dy += (_pressedKeys.Contains(Keys.NumPad9) ? -1 : 0) + (_pressedKeys.Contains(Keys.NumPad3) ? 1 : 0);

			//dy += (_pressedKeys.Contains(Keys.Home) ? -1 : 0) + (_pressedKeys.Contains(Keys.End) ? 1 : 0);
			//dy += (_pressedKeys.Contains(Keys.PageUp) ? -1 : 0) + (_pressedKeys.Contains(Keys.PageDown) ? 1 : 0);

			//if (dx != 0 || dy != 0) World.MoveCommandReceived(dx, dy);
		}


		private void DrawContent(GameTime _gameTime, SpriteBatch _spriteBatch, World _world)
		{
			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			_world.Map.SetData(m_mapTiles, World.Avatar.Point);
			for (int i = 0; i < m_mapTiles.GetLength(0); ++i)
			{
				for (int j = 0; j < m_mapTiles.GetLength(1); ++j)
				{
					m_mapTiles[i, j].Draw(_spriteBatch, i, j);
				}
			}

			Tiles.HeroTile.DrawAtCell(_spriteBatch, m_map.Rectangle.Width / 2, m_map.Rectangle.Height / 2);
			var str = @"Игра будет называться Dwarf Defense. Cмесь Tower Defense (оборона от волн монстров), Dwarf Fortress (строительство укреплений и производство предметов) и все это в виде ala RogueLike (ascii - графика).Описание: вы управляете дфарфом-одиночкой, который прибывает на новое место, начинает строительство своего дома, добывать полезные ископаемые, производить различные предметы и строить оборону (башни, ловушки, големов и т.п.). Затем на его крепость начинают нападать волны различной нечисти (орки, гоблины, драконы, нежить и т.п.), с которыми он справляется в рукопашную и при помощи своей обороны. В том месте, где вы высаживаетесь, так же содержатся подземные пещеры, населенные своими обитателями и заброшенные подземные комплексы/лабиринты.
						Игра будет называться Dwarf Defense. Cмесь Tower Defense (оборона от волн монстров), Dwarf Fortress (строительство укреплений и производство предметов) и все это в виде ala RogueLike (ascii - графика).Описание: вы управляете дфарфом-одиночкой, который прибывает на новое место, начинает строительство своего дома, добывать полезные ископаемые, производить различные предметы и строить оборону (башни, ловушки, големов и т.п.). Затем на его крепость начинают нападать волны различной нечисти (орки, гоблины, драконы, нежить и т.п.), с которыми он справляется в рукопашную и при помощи своей обороны. В том месте, где вы высаживаетесь, так же содержатся подземные пещеры, населенные своими обитателями и заброшенные подземные комплексы/лабиринты.";

			var highlights = new Dictionary<string, Color> { { "Игра", Color.Crimson }, { "вы", Color.Green } };
			m_messages.DrawText(new TextPortion(str, highlights), _spriteBatch, Tile.Font, Color.White);

			_spriteBatch.End();
		}

		private void DrawBackground(GameTime _gameTime, SpriteBatch _spriteBatch)
		{

			// TODO: Add your drawing code here

			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

			DrawHelper.Fill(m_map, _spriteBatch, Tiles.SolidTile, Tiles.GrowndTile.Color);

			_spriteBatch.End();

			//_spriteBatch.Draw(m_sceneTexture, new Vector2(0, 0), Color.Yellow);

			//PreDraw(_spriteBatch);

			//for (int i = 0; i < 64; ++i)
			//    for (int j = 0; j < 64; ++j)
			//    {
			//        m_spriteBatch.DrawTileAtCell(new Tile(0, 12), i, j, Color.Brown);
			//    }

			//_spriteBatch.End();
		}
	}
}
