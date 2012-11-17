using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Essences;
using RusLanguage;

namespace GameUi.UIBlocks
{
	internal class LookAtUiBlock : UiBlockWithText
	{
		private readonly TurnMessageUiBlock m_messages;
		private Point m_avatarScreenPoint;
		private Point m_targetPoint;

		public LookAtUiBlock(TurnMessageUiBlock _messages, Rct _mapRct)
			: base(_mapRct, null, FColor.Gray)
		{
			m_messages = _messages;
			Rebuild();
		}

		public Point TargetPoint
		{
			get { return m_targetPoint; }
			set
			{
				if (m_targetPoint != value)
				{
					m_targetPoint = value;
					MessageManager.SendMessage(this, WorldMessage.JustRedraw);
				}
			}
		}

		public override void Resize(Rct _newRct)
		{
			base.Resize(_newRct);
			Rebuild();
		}

		private void Rebuild()
		{
			TargetPoint = Point.Zero;
			m_avatarScreenPoint = ContentRct.Center;
		}

		public override void KeysPressed(ConsoleKey _key, EKeyModifiers _modifiers)
		{
			var dPoint = KeyTranslator.GetDirection(_key);
			if (dPoint != null)
			{
				var newPoint = TargetPoint + dPoint + m_avatarScreenPoint;
				if (ContentRct.Contains(newPoint))
				{
					TargetPoint += dPoint;
				}
			}
			switch (_key)
			{
				case ConsoleKey.Escape:
				case ConsoleKey.Z:
					CloseTopBlock();
					break;
			}
			MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void DrawBackground() { }

		public override void DrawContent()
		{
			var strings = new List<string> {"[z|Esc] - выход"};

			m_messages.DrawLine(JoinCommandCaptions(strings), FColor.White, 0, 0, EAlignment.LEFT);

			var liveCell = World.TheWorld.Avatar[TargetPoint];
			if (liveCell.IsSeenBefore)
			{
                ETileset.TARGETING.GetTile(0).Draw(TargetPoint + m_avatarScreenPoint, FColor.Gold);

				var lighted = MapUiBlock.GetLighted(liveCell, liveCell.Visibility, World.TheWorld.Avatar.Layer.Ambient);

				var list = new List<string>();
				var s = "";
				if (lighted.Lightness() > World.TheWorld.Avatar.Layer.FogLightness)
				{
					s = "там ";
                    ETileset.TARGETING.GetTile(0).Draw(TargetPoint + m_avatarScreenPoint, FColor.Green);

					if (liveCell.Creature != null)
					{
						list.Add(liveCell.Creature.GetName(World.TheWorld.Avatar, liveCell));
					}
					if (liveCell.Thing != null)
					{
						list.Add(liveCell.Thing.GetName(World.TheWorld.Avatar, liveCell));
					}
					if (liveCell.Items.Count() > 1)
					{
						list.Add("вещи");
					}
					else if (liveCell.Items.Count() == 1)
					{
						list.Add(liveCell.Items.First().GetName(World.TheWorld.Avatar, liveCell));
					}
				}
				else
				{
					s = Variants.ThereIsWas(liveCell.TerrainAttribute.Sex, World.Rnd);
				}
				list.Add(liveCell.TerrainAttribute.DisplayName);
				m_messages.DrawLine(s + string.Join(", ", list), FColor.Gray, 1, 0, EAlignment.LEFT);
			}
			else
			{
                ETileset.TARGETING.GetTile(0).Draw(TargetPoint + m_avatarScreenPoint, FColor.Red);
			}
		}

		public override void MouseMove(Point _pnt) { SetPoint(_pnt); }

		private void SetPoint(Point _pnt)
		{
			TargetPoint = _pnt - m_avatarScreenPoint + ContentRct.LeftTop;
			MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void MouseButtonUp(Point _pnt, EMouseButton _button)
		{
			if (_button != EMouseButton.LEFT) return;

			SetPoint(_pnt);
			KeysPressed(ConsoleKey.M, EKeyModifiers.NONE);
		}
	}
}