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
		private readonly ThingsPresenter m_presenter;

		public SelectItemsUiBlock(Rectangle _rectangle, IEnumerable<ThingDescriptor> _items, Act _act)
			: base(_rectangle, Frame.GoldFrame, Color.DeepSkyBlue, Fonts.Font)
		{
			m_act = _act;
			m_presenter = new ThingsPresenter(_items);
			var count = m_presenter.Items.OfType<ThingDescriptorFromCollection>().Count();
			var whereCnt = m_presenter.Items.OfType<WhereDescriptorFromCollection>().Count();
			if (whereCnt > 1) count += whereCnt*2 - 1;
			Rectangle = new Rectangle(_rectangle.Width/2, _rectangle.Top, _rectangle.Width/2,
			                          3 + (int) (count*m_lineHeight)/Tile.Size);
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
				var items = m_presenter.Items.OfType<ThingDescriptorFromCollection>().Where(_itemFromCollection => _itemFromCollection.IsChecked).Select(_collection => _collection.ThingDescriptor);
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

	internal class ThingsPresenter
	{
		private readonly List<IDescriptorFromCollection> m_items = new List<IDescriptorFromCollection>();

		public ThingsPresenter(IEnumerable<ThingDescriptor> _descriptors)
		{
			var key = ConsoleKey.A;
			var containers = _descriptors.Select(_descriptor => _descriptor.Container).Distinct();
			foreach (var container in containers)
			{
				var cntnr = container;

				if (containers.Count() > 1)
				{
					m_items.Add(new WhereDescriptorFromCollection(cntnr));
				}
				var local = _descriptors.Where(_descriptor => _descriptor.Container == cntnr).ToArray();
				var hacheCodes = local.Select(_item => _item.GetHashCode()).Distinct();
				var list = hacheCodes.Select(_code => local.First(_item => _item.GetHashCode() == _code)).OrderBy(_item => _item.UiOrderIndex).ToList();
				foreach (var item in list)
				{
					m_items.Add(new ThingDescriptorFromCollection(key, item, _descriptors));
					key++;
				}
			}
		}

		public IEnumerable<IDescriptorFromCollection> Items
		{
			get { return m_items; }
		}

		public void KeysPressed(ConsoleKey _key)
		{
			foreach (var fromCollection in m_items.OfType<ThingDescriptorFromCollection>().Where(_itemFromCollection => _itemFromCollection.Key == _key))
			{
				fromCollection.IsChecked = !fromCollection.IsChecked;
			}
		}
	}

	internal interface IDescriptorFromCollection
	{
		void DrawLine(int _line, SpriteBatch _spriteBatch, SelectItemsUiBlock _selectItemsUiBlock);
	}

	internal class WhereDescriptorFromCollection : IDescriptorFromCollection
	{
		private readonly Container m_container;

		public WhereDescriptorFromCollection(Container _container)
		{
			m_container = _container;
		}

		public virtual void DrawLine(int _line, SpriteBatch _spriteBatch, SelectItemsUiBlock _selectItemsUiBlock)
		{
			var cntnr = m_container == null ? "на земле" : m_container.Name;
			_selectItemsUiBlock.DrawLine("*** " + cntnr.ToUpper() + " ***", Color.White, _spriteBatch, _line, 0, UIBlock.EAlignment.CENTER);
		}
	}

	internal class ThingDescriptorFromCollection : IDescriptorFromCollection
	{
		private readonly IEnumerable<ThingDescriptor> m_descriptors;
		private readonly ThingDescriptor m_thingDescriptor;
		private readonly ConsoleKey m_key;

		public ThingDescriptorFromCollection(ConsoleKey _key, ThingDescriptor _thingDescriptor, IEnumerable<ThingDescriptor> _descriptors)
		{
			m_key = _key;
			m_thingDescriptor = _thingDescriptor;
			m_descriptors = _descriptors;
		}

		public bool IsChecked { get; set; }

		public ConsoleKey Key
		{
			get { return m_key; }
		}

		public ThingDescriptor ThingDescriptor
		{
			get { return m_thingDescriptor; }
		}

		public string Text
		{
			get { return m_descriptors.Where(_descriptor => _descriptor.GetHashCode() == m_thingDescriptor.GetHashCode()).Count() + " " + m_thingDescriptor.Thing.Name; }
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