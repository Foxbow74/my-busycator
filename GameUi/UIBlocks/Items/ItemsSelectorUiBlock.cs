using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal abstract class ItemsSelectorUiBlock : UiBlockWithText
	{
		private readonly Act m_act;
		private readonly ESelectItemDialogBehavior m_behavior;
		private readonly IEnumerable<ThingDescriptor> m_descriptors;

		private readonly Dictionary<Tuple<ConsoleKey, EKeyModifiers>, EThingCategory> m_filters =
			new Dictionary<Tuple<ConsoleKey, EKeyModifiers>, EThingCategory>();

		private char m_currentFilter = '*';

		private int m_currentPage;
		private Dictionary<int, List<ILinePresenter>> m_pages;

		protected ItemsSelectorUiBlock(Rectangle _rectangle, ESelectItemDialogBehavior _behavior, Act _act,
		                               IEnumerable<ThingDescriptor> _descriptors)
			: base(_rectangle, Frame.GoldFrame, Color.Green.ToFColor())
		{
			m_behavior = _behavior;
			m_act = _act;
			m_currentFilter = '*';
			m_descriptors = _descriptors;
		}

		/// <summary>
		/// 	Empty if no filter
		/// </summary>
		protected abstract IEnumerable<EThingCategory> AllowedCategories { get; }

		protected abstract int HeaderTakesLine { get; }

		protected void Rebuild()
		{
			var key = ConsoleKey.A;

			var linesPerPage = TextLinesMax - HeaderTakesLine;
			var currentCategory = "";

			var done = new List<int>();
			List<ILinePresenter> page = null;

			m_pages.Clear();

			var categories =
				m_descriptors.Select(_descriptor => _descriptor.Thing.Category).Distinct().OrderBy(_category => _category);

			if (AllowedCategories.Any())
			{
				categories = categories.Intersect(AllowedCategories).OrderBy(_category => _category);
			}

			foreach (var cat in categories)
			{
				var attribute = ThingCategoryAttribute.GetAttribute(cat);
				if (m_currentFilter != '*' && attribute.C != m_currentFilter) continue;
				foreach (var descriptor in m_descriptors.Where(_descriptor => _descriptor.Thing.Category == cat))
				{
					if (done.Contains(descriptor.Thing.GetHashCode())) continue;
					done.Add(descriptor.Thing.GetHashCode());

					if (page == null)
					{
						page = new List<ILinePresenter>();
						m_pages.Add(m_pages.Count, page);
						currentCategory = "";
						key = ConsoleKey.A;
					}
					if (attribute.DisplayName != currentCategory)
					{
						page.Add(new ThingCategoryPresenter(cat));
						currentCategory = attribute.DisplayName;
					}
					page.Add(new ThingPresenter(key, descriptor, m_descriptors));
					key++;
					if (page.Count == linesPerPage - 1)
					{
						page = null;
					}
				}
			}
		}

		protected abstract void DrawHeader();

		public override void DrawContent()
		{
			if (m_pages == null)
			{
				m_pages = new Dictionary<int, List<ILinePresenter>>();
				Rebuild();
			}

			DrawHeader();

			var line = HeaderTakesLine;

			var bottomString = new List<string> {"[PgUp/PgDown] листать", "[z|Esc] - выход"};

			if ((m_behavior & ESelectItemDialogBehavior.SELECT_MULTIPLE) == ESelectItemDialogBehavior.SELECT_MULTIPLE)
			{
				bottomString.Insert(1, "[Enter] выбрать");
			}

			if (m_pages.Count > 0)
			{
				var page = m_pages[m_currentPage];
				foreach (var presenter in page)
				{
					if (presenter is ThingCategoryPresenter)
					{
						var category = ((ThingCategoryPresenter) presenter).Category;
						var attribute = ThingCategoryAttribute.GetAttribute(category);
						m_filters[new Tuple<ConsoleKey, EKeyModifiers>(attribute.Key, attribute.Modifiers)] = category;
					}
					presenter.DrawLine(line++, this);
				}
				if ((m_behavior & ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER) == ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER)
				{
					var filters = "*" +
					              string.Join("",
					                          m_filters.Values.Select(
					                          	_category => ThingCategoryAttribute.GetAttribute(_category).C.ToString()));
					bottomString.Insert(0, "[" + filters + "] фильтровать");
				}
			}
			else
			{
				DrawLine("(ничего нет)", Color.White.ToFColor(), line, 0, EAlignment.CENTER);
			}
			DrawLine(JoinCommandCaptions(bottomString), ForeColor, TextLinesMax - 2, 0, EAlignment.CENTER);
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			if ((m_behavior & ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER) == ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER)
			{
				if (_key == ConsoleKey.Multiply || (_key == ConsoleKey.D8 && _modifiers == EKeyModifiers.SHIFT))
				{
					m_currentFilter = '*';
					Rebuild();
					return;
				}
				foreach (var pair in m_filters)
				{
					if (pair.Key.Item1 == _key && pair.Key.Item2 == _modifiers)
					{
						m_currentFilter = ThingCategoryAttribute.GetAttribute(pair.Value).C;
						Rebuild();
						return;
					}
				}
			}

			if (_modifiers != EKeyModifiers.NONE) return;

			switch (_key)
			{
				case ConsoleKey.Z:
				case ConsoleKey.Escape:
					CloseTopBlock();
					return;
				case ConsoleKey.PageUp:
					m_currentPage = Math.Max(0, m_currentPage - 1);
					break;
				case ConsoleKey.PageDown:
					m_currentPage = Math.Min(m_pages.Count - 1, m_currentPage + 1);
					break;
				case ConsoleKey.Enter:
					if ((m_behavior & ESelectItemDialogBehavior.SELECT_MULTIPLE) == ESelectItemDialogBehavior.SELECT_MULTIPLE)
					{
						CloseTopBlock(_key);
					}
					return;
			}

			if (m_pages.Count > 0)
			{
				foreach (var presenter in m_pages[m_currentPage].OfType<ThingPresenter>())
				{
					if (presenter.Key == _key)
					{
						presenter.IsChecked = !presenter.IsChecked;
						if ((m_behavior & ESelectItemDialogBehavior.SELECT_ONE) == ESelectItemDialogBehavior.SELECT_ONE)
						{
							CloseTopBlock(ConsoleKey.Enter);
							return;
						}
					}
				}
			}
		}

		protected override void OnClosing(ConsoleKey _consoleKey)
		{
			if (_consoleKey == ConsoleKey.Enter)
			{
				var presenters =
					m_pages.SelectMany(_pair => _pair.Value).OfType<ThingPresenter>().Where(_presenter => _presenter.IsChecked);

				if (presenters.Any())
				{
					foreach (var linePresenter in presenters)
					{
						if (linePresenter.IsChecked)
						{
							AddCheckedItemToResult(linePresenter.ThingDescriptor);
						}
					}
					return;
				}
			}
			if (m_act != null)
			{
				m_act.AddParameter(ThingDescriptor.Empty);
			}
		}

		protected virtual void AddCheckedItemToResult(ThingDescriptor _thingDescriptor)
		{
			if (m_act != null)
			{
				m_act.AddParameter(_thingDescriptor);
			}
		}
	}
}