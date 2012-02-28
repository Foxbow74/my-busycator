using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using GameCore;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class ThingPresenter : ILinePresenter
	{
		private readonly IEnumerable<ThingDescriptor> m_descriptors;
		private readonly ConsoleKey m_key;
		private readonly ThingDescriptor m_thingDescriptor;

		public ThingPresenter(ConsoleKey _key, ThingDescriptor _thingDescriptor, IEnumerable<ThingDescriptor> _descriptors)
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
			get
			{
				return m_thingDescriptor;
			}
		}

		public int Count
		{
			get
			{
				return 	m_descriptors.Count(_descriptor => _descriptor.GetName(World.TheWorld.Avatar) == m_thingDescriptor.GetName(World.TheWorld.Avatar));
			}
		}

		public string Text
		{
			get
			{
				var count = Count;
				return (count > 1 ? count.ToString(CultureInfo.InvariantCulture) : "") + " " + m_thingDescriptor.GetName(World.TheWorld.Avatar);
			}
		}

		#region ILinePresenter Members

		public void DrawLine(int _line, UiBlockWithText _uiBlock)
		{
			_uiBlock.DrawLine("+", IsChecked ? Color.Yellow.ToFColor() : Color.Black.ToFColor(), _line, 10, EAlignment.LEFT);
			_uiBlock.DrawLine(Enum.GetName(typeof(ConsoleKey), Key), Color.White.ToFColor(), _line, 20,
			                  EAlignment.LEFT);
			_uiBlock.DrawLine(Text, Color.DarkGray.ToFColor(), _line, 40, EAlignment.LEFT);
		}

		#endregion
	}
}