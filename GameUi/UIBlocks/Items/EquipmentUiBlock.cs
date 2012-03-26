using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Creatures;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks.Items
{
	internal class EquipmentUiBlock : UiBlockWithText
	{
		private readonly Intelligent m_intelligent;
		private readonly List<EquipmentPresenter> m_presenters = new List<EquipmentPresenter>();

		public EquipmentUiBlock(Rct _rct)
			: base(_rct, Frame.SimpleFrame, FColor.White)
		{
			m_intelligent = World.TheWorld.Avatar;
			Rebuild();
		}

		public Intelligent Intelligent { get { return m_intelligent; } }

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

		public override void DrawContent()
		{
			DrawLine("СНАРЯЖЕНИЕ", ForeColor, 0, 0, EAlignment.CENTER);
			DrawLine("ВЕС:", ForeColor, 2, 0, EAlignment.LEFT);
			DrawLine("ДОСТУПНЫЙ ВЕС:", ForeColor, 2, 0, EAlignment.RIGHT);

			var line = 4;
			foreach (var linePresenter in m_presenters)
			{
				linePresenter.DrawLine(line++, this);
			}

			DrawLine(
				"[A-" + m_presenters.Max(_presenter => _presenter.C) +
				"] Надеть/снять предмет   -   [V] Рюкзак   -   [z|Esc] - выход",
				ForeColor,
				TextLinesMax - 2,
				20,
				EAlignment.CENTER);
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
					MessageManager.SendMessage(this, new OpenUIBlockMessage(new BackpackUiBlock(Rct)));
					return;
			}
			var presenter = m_presenters.SingleOrDefault(_presenter => _presenter.Key == _key);

			if (presenter != null)
			{
				if (presenter.Item == null)
				{
					MessageManager.SendMessage(this, new OpenUIBlockMessage(new SelectToTakeOnUiBlock(Rct, this, presenter)));
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
		public static float MaxIndent;

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

		public void DrawLine(int _line, UiBlockWithText _uiBlock)
		{
			_uiBlock.DrawLine(C.ToString(), FColor.White, _line, 20, EAlignment.LEFT);
			var indent =
				_uiBlock.DrawLine(EquipmentPlacesAttribute.GetAttribute(Place).DisplayName,
				                  FColor.Gray,
				                  _line,
				                  40,
				                  EAlignment.LEFT) + 2;
			MaxIndent = Math.Max(MaxIndent, indent);
			indent = _uiBlock.DrawLine(":", FColor.DarkGray, _line, MaxIndent, EAlignment.LEFT) + 5;
			if (Item == null)
			{
				_uiBlock.DrawLine("-", FColor.DarkGray, _line, indent, EAlignment.LEFT);
			}
			else
			{
				_uiBlock.DrawLine(Item.GetName(World.TheWorld.Avatar), FColor.DarkGray, _line, indent, EAlignment.LEFT);
			}
		}

		#endregion
	}
}