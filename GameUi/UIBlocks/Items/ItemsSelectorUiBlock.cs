using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Essences;

namespace GameUi.UIBlocks.Items
{
	internal abstract class ItemsSelectorUiBlock : UiBlockWithText
	{
		private readonly Act m_act;
		private readonly ESelectItemDialogBehavior m_behavior;
		private readonly IEnumerable<EssenceDescriptor> m_descriptors;

		private readonly Dictionary<Tuple<ConsoleKey, EKeyModifiers>, EItemCategory> m_filters =
			new Dictionary<Tuple<ConsoleKey, EKeyModifiers>, EItemCategory>();

		private char m_currentFilter = '*';

		private int m_currentPage;
		private Dictionary<int, List<ILinePresenter>> m_pages;

		protected ItemsSelectorUiBlock(Rct _rct,
		                               ESelectItemDialogBehavior _behavior,
		                               Act _act,
		                               IEnumerable<EssenceDescriptor> _descriptors)
			: base(_rct, Frame.Frame2, FColor.Green)
		{
			m_behavior = _behavior;
			m_act = _act;
			m_currentFilter = '*';
			m_descriptors = _descriptors;
		}

		/// <summary>
		/// 	Empty if no filter
		/// </summary>
		protected abstract IEnumerable<EItemCategory> AllowedCategories { get; }

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
				m_descriptors.Select(_descriptor => _descriptor.Essence).OfType<Item>().Select(_item => _item.Category).Distinct().OrderBy(_category => _category);

			if (AllowedCategories.Any())
			{
				categories = categories.Intersect(AllowedCategories).OrderBy(_category => _category);
			}

			foreach (var cat in categories)
			{
				var attribute = EssenceCategoryAttribute.GetAttribute(cat);
				if (m_currentFilter != '*' && attribute.C != m_currentFilter) continue;
				foreach (var descriptor in m_descriptors)
				{
					var item = descriptor.Essence as Item;

					if(item==null) continue;

					if (done.Contains(item.GetHashCode())) continue;
					done.Add(item.GetHashCode());

					if (page == null)
					{
						page = new List<ILinePresenter>();
						m_pages.Add(m_pages.Count, page);
						currentCategory = "";
						key = ConsoleKey.A;
					}
					var name = EALSentence.GENERAL.GetString(cat.AsNoun());
					if (name != currentCategory)
					{
						page.Add(new EssenceCategoryPresenter(cat));
						currentCategory = name;
					}
					page.Add(new EssencePresenter(key, descriptor, m_descriptors));
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

			var bottomString = new List<string> { "[PgUp/PgDown] листать", "[z|Esc] - " + EALConst.EXIT.GetString() };

			if ((m_behavior & ESelectItemDialogBehavior.SELECT_MULTIPLE) == ESelectItemDialogBehavior.SELECT_MULTIPLE)
			{
				bottomString.Insert(1, "[Enter] выбрать");
			}

			if (m_pages.Count > 0)
			{
				var page = m_pages[m_currentPage];
				foreach (var presenter in page)
				{
					if (presenter is EssenceCategoryPresenter)
					{
						var category = ((EssenceCategoryPresenter) presenter).Category;
						var attribute = EssenceCategoryAttribute.GetAttribute(category);
						m_filters[new Tuple<ConsoleKey, EKeyModifiers>(attribute.Key, attribute.Modifiers)] = category;
					}
					presenter.DrawLine(line++, this);
				}
				if ((m_behavior & ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER) == ESelectItemDialogBehavior.ALLOW_CHANGE_FILTER)
				{
					var filters = "*" +
					              string.Join("",
					                          m_filters.Values.Select(
					                          	_category => EssenceCategoryAttribute.GetAttribute(_category).C.ToString()));
					bottomString.Insert(0, "[" + filters + "] фильтровать");
				}
			}
			else
			{
				DrawLine("(ничего нет)", FColor.White, line, 0, EAlignment.CENTER);
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
						m_currentFilter = EssenceCategoryAttribute.GetAttribute(pair.Value).C;
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
				foreach (var presenter in m_pages[m_currentPage].OfType<EssencePresenter>())
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
			base.OnClosing(_consoleKey);
			if (_consoleKey == ConsoleKey.Enter)
			{
				var presenters =
					m_pages.SelectMany(_pair => _pair.Value).OfType<EssencePresenter>().Where(_presenter => _presenter.IsChecked);

				if (presenters.Any())
				{
					foreach (var linePresenter in presenters)
					{
						if (linePresenter.IsChecked)
						{
							AddCheckedItemToResult(linePresenter.EssenceDescriptor);
						}
					}
					return;
				}
			}
			if (m_act != null)
			{
				m_act.AddParameter(EssenceDescriptor.Empty);
			}
		}

		protected virtual void AddCheckedItemToResult(EssenceDescriptor _essenceDescriptor)
		{
			if (m_act != null)
			{
				m_act.AddParameter(_essenceDescriptor);
			}
		}
	}
}