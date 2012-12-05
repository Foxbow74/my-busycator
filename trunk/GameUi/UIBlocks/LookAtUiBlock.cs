using System;
using System.Collections.Generic;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Messages;
using GameCore.Misc;

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
					//MessageManager.SendMessage(this, WorldMessage.JustRedraw);
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
			//MessageManager.SendMessage(this, WorldMessage.JustRedraw);
		}

		public override void DrawBackground() { }

		public override void DrawContent()
		{
			var strings = new List<string> { "[z|Esc] - " + EALConst.EXIT.GetString() };

			m_messages.DrawLine(JoinCommandCaptions(strings), FColor.White, 0, 0, EAlignment.LEFT);

			var liveCell = World.TheWorld.Avatar[TargetPoint];
			if (liveCell.IsSeenBefore)
			{
                ETileset.TARGETING.GetTile(0).Draw(TargetPoint + m_avatarScreenPoint, FColor.Gold);

				var lighted = liveCell.FinalLighted;

				var list = new List<Noun>();
				if (lighted.Lightness() > World.TheWorld.Avatar.GeoInfo.Layer.FogLightness)
				{
                    ETileset.TARGETING.GetTile(0).Draw(TargetPoint + m_avatarScreenPoint, FColor.Green);

					if (liveCell.Creature != null)
					{
						list.Add(liveCell.Creature.Name);
					}
					if (liveCell.Thing != null)
					{
						list.Add(liveCell.Thing.Name);
					}
					foreach (var item in liveCell.Items)
					{
						list.Add(item.Name);
					}
					m_messages.DrawLine(EALSentence.THERE_ARE.GetString(list.ToArray()), FColor.Gray, 1, 0, EAlignment.LEFT);
				}
				else
				{
					m_messages.DrawLine(EALSentence.THERE_ARE_WERE.GetString(list.ToArray()), FColor.Gray, 1, 0, EAlignment.LEFT);
				}
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