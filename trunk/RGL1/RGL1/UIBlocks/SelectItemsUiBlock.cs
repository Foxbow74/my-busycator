using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.UIBlocks.SelectItemPresenter;
using RGL1.UIBlocks.ThingPresenter;

namespace RGL1.UIBlocks
{
	internal class SelectItemsUiBlock : UIBlock
	{
		private readonly Act m_act;
		private readonly ThingsPresenter m_presenter;

		public SelectItemsUiBlock(Rectangle _rectangle, IEnumerable<ThingDescriptor> _items, Act _act)
			: base(_rectangle, Frame.GoldFrame, Color.DeepSkyBlue, Fonts.Font)
		{
			m_act = _act;
			m_presenter = new ThingsPresenter(_items.ToList());
			var count = m_presenter.Items.OfType<ThingDescriptorFromCollection>().Count();
			var whereCnt = m_presenter.Items.OfType<WhereDescriptorFromCollection>().Count();
			if (whereCnt > 1) count += whereCnt*2 - 1;
			Rectangle = new Rectangle(_rectangle.Width/2, _rectangle.Top, _rectangle.Width/2,
			                          3 + (int) (count*m_lineHeight)/Tile.Size);
			UpdateContentRectangle();
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if (_modifiers != EKeyModifiers.NONE) return;
			if (_key == ConsoleKey.Escape)
			{
				m_act.IsCancelled = true;
				CloseTopBlock();
				return;
			}
			
			if (_key == ConsoleKey.Enter)
			{
				var items = m_presenter.Items.OfType<ThingDescriptorFromCollection>().Where(_itemFromCollection => _itemFromCollection.IsChecked).Select(_collection => _collection.ThingDescriptor).ToList();
				if (items.Count>0)
				{
					foreach (var item in items)
					{
						m_act.AddParameter(item);
					}
					CloseTopBlock();
					return;
				}
				m_act.IsCancelled = true;
			}
			else
			{
				m_presenter.KeysPressed(_key);
			}
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();

			var line = 0;
			foreach (var item in m_presenter.Items)
			{
				if (line>0 && item is WhereDescriptorFromCollection) line++;
				item.DrawLine(line, _spriteBatch, this);
				line++;
			}
			_spriteBatch.End();
		}
	}
}