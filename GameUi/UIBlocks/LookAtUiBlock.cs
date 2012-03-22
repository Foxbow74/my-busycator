using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.Objects;

namespace GameUi.UIBlocks
{
	class LookAtUiBlock: UiBlockWithText
	{
		private Point m_avatarScreenPoint;
		private Point m_halfScreen;
		private readonly TurnMessageUiBlock m_messages;
		private Point m_targetPoint;

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

		public LookAtUiBlock(TurnMessageUiBlock _messages, Rct _mapRct)
			: base(_mapRct, null, FColor.Gray)
		{
			m_messages = _messages;
			Rebuild();
		}

		private void Rebuild()
		{
			TargetPoint = Point.Zero;
			m_halfScreen = new Point(ContentRct.Width, ContentRct.Height)/2;
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

		public override void DrawBackground()
		{
		}

		public override void DrawContent()
		{
			var strings = new List<string> {"[z|Esc] - выход"};

			m_messages.DrawLine(JoinCommandCaptions(strings), FColor.White, 0, 0, EAlignment.LEFT);

			var liveCell = World.TheWorld.Avatar[TargetPoint];
			if (liveCell.IsSeenBefore)
			{
				ETiles.TARGET_CROSS.GetTile().Draw(TargetPoint + m_avatarScreenPoint, FColor.Gold);

				var lighted = MapUiBlock.GetLighted(liveCell, liveCell.Visibility, World.TheWorld.Avatar.Layer.Ambient);

				var s = new StringBuilder();
				s.Append(liveCell.TerrainAttribute.DisplayName); 
				if (lighted.Lightness() > World.TheWorld.Avatar.Layer.FogLightness)
				{
					ETiles.TARGET_CROSS.GetTile().Draw(TargetPoint + m_avatarScreenPoint, FColor.Green);

					if(liveCell.Creature!=null)
					{
						s.Append(", " + liveCell.Creature.GetName(World.TheWorld.Avatar, liveCell));
					}
					if (liveCell.Furniture != null)
					{
						s.Append(", " + liveCell.Furniture.GetName(World.TheWorld.Avatar, liveCell));
					}
					if (liveCell.Items.Count() > 1)
					{
						s.Append(", вещи");
					}
					else if(liveCell.Items.Count()==1)
					{
						s.Append(", " + liveCell.Items.First().GetName(World.TheWorld.Avatar, liveCell));
					}
				}
				else
				{

				}
				m_messages.DrawLine(s.ToString(), FColor.Gray, 1, 0, EAlignment.LEFT);
			}
			else
			{
				ETiles.TARGET_CROSS.GetTile().Draw(TargetPoint + m_avatarScreenPoint, FColor.Red);
			}
		}

		public override void MouseMove(Point _pnt)
		{
			SetPoint(_pnt);
		}

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
