using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks
{
	internal class SelectItemsUiBlock : UIBlock
	{
		private readonly Act m_act;
		private readonly ItemsPresenter m_presenter;

		public SelectItemsUiBlock(Rectangle _rectangle, ItemsCollection _itemsCollection, Act _act)
			: base(_rectangle, Frame.GoldFrame, Color.DeepSkyBlue, Fonts.Font)
		{
			m_act = _act;
			m_presenter = new ItemsPresenter(_itemsCollection);
			Rectangle = new Rectangle(_rectangle.Width/2, _rectangle.Top, _rectangle.Width/2,
			                          5 + (int) (m_presenter.Items.Count()*m_lineHeight)/Tile.Size);
			UpdateContentRectangle();
		}

		public override void DrawFrame(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			base.DrawFrame(_spriteBatch);
			_spriteBatch.End();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_modifiers != EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Escape)
			{
				var items =
					m_presenter.Items.Where(_itemFromCollection => _itemFromCollection.IsChecked).Select(
						_collection => _collection.Item);
				if (items.Any())
				{
					m_act.AddParameter(items);
				}
				else
				{
					m_act.IsCancelled = true;
				}
				CloseTopBlock();
			}
			m_presenter.KeysPressed(_key);
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();

			var line = 1;
			foreach (var itemFromCollection in m_presenter.Items)
			{
				itemFromCollection.DrawLine(line, _spriteBatch, this);
				line++;
			}
			_spriteBatch.End();
		}
	}

	internal class ItemsPresenter
	{
		private readonly List<ItemFromCollection> m_items = new List<ItemFromCollection>();

		public ItemsPresenter(ItemsCollection _collection)
		{
			var codes = _collection.Items.Select(_item => _item.GetHashCode()).Distinct();
			var list =
				codes.Select(_code => _collection.Items.First(_item => _item.GetHashCode() == _code)).OrderBy(
					_item => _item.UiOrderIndex).ToList();

			var key = ConsoleKey.A;
			foreach (var item in list)
			{
				m_items.Add(new ItemFromCollection(key, item, _collection));
				key++;
			}
		}

		public IEnumerable<ItemFromCollection> Items
		{
			get { return m_items; }
		}

		public void KeysPressed(ConsoleKey _key)
		{
			foreach (var fromCollection in m_items.Where(_itemFromCollection => _itemFromCollection.Key == _key))
			{
				fromCollection.IsChecked = !fromCollection.IsChecked;
			}
		}
	}

	internal class ItemFromCollection
	{
		private readonly ItemsCollection m_collection;
		private readonly Item m_item;
		private readonly ConsoleKey m_key;

		public ItemFromCollection(ConsoleKey _key, Item _item, ItemsCollection _collection)
		{
			m_key = _key;
			m_item = _item;
			m_collection = _collection;
		}

		public bool IsChecked { get; set; }

		public ConsoleKey Key
		{
			get { return m_key; }
		}

		public Item Item
		{
			get { return m_item; }
		}

		public string Text
		{
			get { return m_collection.Items.Where(_item => _item.GetHashCode() == m_item.GetHashCode()).Count() + " " + m_item.Name; }
		}

		public void DrawLine(int _line, SpriteBatch _spriteBatch, SelectItemsUiBlock _selectItemsUiBlock)
		{
			_selectItemsUiBlock.DrawLine("+", IsChecked ? Color.Yellow : Color.Black, _spriteBatch, _line, 10,
			                             UIBlock.EAlignment.LEFT);
			_selectItemsUiBlock.DrawLine(Enum.GetName(typeof (ConsoleKey), Key), Color.White, _spriteBatch, _line, 30,
			                             UIBlock.EAlignment.LEFT);
			_selectItemsUiBlock.DrawLine(Text, Color.Gray, _spriteBatch, _line, 50, UIBlock.EAlignment.LEFT);
		}
	}
}