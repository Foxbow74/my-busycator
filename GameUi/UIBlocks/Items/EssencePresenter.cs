using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GameCore;
using GameCore.Essences;

namespace GameUi.UIBlocks.Items
{
	internal class EssencePresenter : ILinePresenter
	{
		private readonly IEnumerable<EssenceDescriptor> m_descriptors;
		private readonly ConsoleKey m_key;
		private readonly EssenceDescriptor m_essenceDescriptor;

		public EssencePresenter(ConsoleKey _key, EssenceDescriptor _essenceDescriptor, IEnumerable<EssenceDescriptor> _descriptors)
		{
			m_key = _key;
			m_essenceDescriptor = _essenceDescriptor;
			m_descriptors = _descriptors;
		}

		public bool IsChecked { get; set; }

		public ConsoleKey Key { get { return m_key; } }

		public EssenceDescriptor EssenceDescriptor { get { return m_essenceDescriptor; } }

		public int Count { get { return m_descriptors.Count(_descriptor => _descriptor.Essence.GetName(World.TheWorld.Avatar) == m_essenceDescriptor.Essence.GetName(World.TheWorld.Avatar)); } }

		public string Text
		{
			get
			{
				var count = Count;
				return (count > 1 ? count.ToString(CultureInfo.InvariantCulture) : "") + " " + m_essenceDescriptor.Essence.GetName(World.TheWorld.Avatar);
			}
		}

		#region ILinePresenter Members

		public void DrawLine(int _line, UiBlockWithText _uiBlock)
		{
			_uiBlock.DrawLine("+", IsChecked ? FColor.Yellow : FColor.Black, _line, 10, EAlignment.LEFT);
			_uiBlock.DrawLine(Enum.GetName(typeof (ConsoleKey), Key),
			                  FColor.White,
			                  _line,
			                  20,
			                  EAlignment.LEFT);
			_uiBlock.DrawLine(Text, FColor.DarkGray, _line, 40, EAlignment.LEFT);
		}

		#endregion
	}
}