using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GameCore.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RGL1.UIBlocks.ThingPresenter
{
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
			get
			{
				var count = m_descriptors.Count(_descriptor => _descriptor.GetHashCode() == m_thingDescriptor.GetHashCode());
				return (count>1?count.ToString(CultureInfo.InvariantCulture):"") + " " + m_thingDescriptor.Thing.Name;
			}
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