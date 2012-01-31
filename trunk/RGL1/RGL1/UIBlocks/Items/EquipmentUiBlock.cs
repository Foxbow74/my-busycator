using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.Items
{
	internal class EquipmentUiBlock : UIBlock
	{
		private readonly Intelligent m_intelligent;
		private readonly List<EquipmentPresenter> m_presenters = new List<EquipmentPresenter>();

		public EquipmentUiBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.SimpleFrame, Color.White, Fonts.Font)
		{
			m_intelligent = World.TheWorld.Avatar;
			Rebuild();
		}

		public Intelligent Intelligent
		{
			get { return m_intelligent; }
		}

		public void Rebuild()
		{
			m_presenters.Clear();
			var key = ConsoleKey.A;
			var c = 'A';
			foreach (var tuple in Intelligent.GetEquipment())
			{
				m_presenters.Add(new EquipmentPresenter(tuple.Item1, tuple.Item2, key++, c++));
			}
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();

			DrawLine("СНАРЯЖЕНИЕ", Color, _spriteBatch, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", Color, _spriteBatch, 2, 0, EAlignment.RIGHT);

			var line = 4;
			foreach (var linePresenter in m_presenters)
			{
				linePresenter.DrawLine(line++, _spriteBatch, this);
			}

			DrawLine(
				"[A-" + m_presenters.Max(_presenter => _presenter.C) +
				"] Надеть/снять предмет   -   [V] Рюкзак   -   [z|Esc] - выход", Color, _spriteBatch, TextLinesMax - 2, 20,
				EAlignment.CENTER);

			_spriteBatch.End();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_modifiers != EKeyModifiers.NONE) return;
			switch (_key)
			{
				case ConsoleKey.Escape:
				case ConsoleKey.Z:
					CloseTopBlock();
					return;
				case ConsoleKey.V:
					MessageManager.SendMessage(this, new OpenUIBlockMessage(new BackpackUiBlock(Rectangle)));
					return;
			}
			var presenter = m_presenters.SingleOrDefault(_presenter => _presenter.Key == _key);

			if (presenter != null)
			{
				if (presenter.Item == null)
				{
					MessageManager.SendMessage(this, new OpenUIBlockMessage(new SelectToTakeOnUiBlock(Rectangle, this, presenter)));
				}
				else
				{
					Intelligent.TakeOff(presenter.Place);
					Rebuild();
				}
			}
		}
	}

	internal class EquipmentPresenter : ILinePresenter
	{
		public static float m_maxIndent;

		public EquipmentPresenter(EEquipmentPlaces _place, Item _item, ConsoleKey _key, char _c)
		{
			Place = _place;
			Item = _item;
			Key = _key;
			C = _c;
		}

		public char C { get; private set; }

		public ConsoleKey Key { get; private set; }

		public Item Item { get; private set; }

		public EEquipmentPlaces Place { get; private set; }

		#region ILinePresenter Members

		public void DrawLine(int _line, SpriteBatch _spriteBatch, UIBlock _uiBlock)
		{
			_uiBlock.DrawLine(C.ToString(), Color.White, _spriteBatch, _line, 20, UIBlock.EAlignment.LEFT);
			var indent =
				_uiBlock.DrawLine(EquipmentPlacesAttribute.GetAttribute(Place).DisplayName, Color.Gray, _spriteBatch, _line, 40,
				                  UIBlock.EAlignment.LEFT) + 2;
			m_maxIndent = Math.Max(m_maxIndent, indent);
			indent = _uiBlock.DrawLine(":", Color.DarkGray, _spriteBatch, _line, m_maxIndent, UIBlock.EAlignment.LEFT) + 5;
			if (Item == null)
			{
				_uiBlock.DrawLine("-", Color.DarkGray, _spriteBatch, _line, indent, UIBlock.EAlignment.LEFT);
			}
			else
			{
				_uiBlock.DrawLine(Item.Name, Color.DarkGray, _spriteBatch, _line, indent, UIBlock.EAlignment.LEFT);
			}
		}

		#endregion
	}
}