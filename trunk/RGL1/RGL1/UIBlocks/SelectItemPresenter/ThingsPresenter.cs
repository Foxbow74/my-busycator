using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Objects;
using RGL1.UIBlocks.SelectItemPresenter;

namespace RGL1.UIBlocks.ThingPresenter
{
	internal class ThingsPresenter
	{
		private readonly List<IDescriptorFromCollection> m_items = new List<IDescriptorFromCollection>();

		public ThingsPresenter(List<ThingDescriptor> _descriptors)
		{
			var key = ConsoleKey.A;
			if (_descriptors == null) return;
			
			var containers = _descriptors.Select(_descriptor => _descriptor.Container).Distinct().ToList();
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
}