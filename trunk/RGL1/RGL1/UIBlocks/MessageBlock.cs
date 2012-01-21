using System.Collections.Generic;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class MessageBlock:UIBlock
	{
		private readonly List<TextPortion.TextLine> m_lines = new List<TextPortion.TextLine>();
		private readonly List<TextPortion> m_portions = new List<TextPortion>();

		public MessageBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.GoldFrame, Color.Yellow)
		{
			MessageManager.NewMessage += MessageManagerNewMessage;
		}

		void MessageManagerNewMessage(object _sender, Message _message)
		{
			if(_message is TextMessage)
			{
				var tm = (TextMessage) _message;
				m_portions.Add(tm.Text);
				tm.Text.SplitByLines(Rectangle.Width - Tile.Size, Tile.Font, 0);
				m_lines.AddRange(tm.Text.TextLines);
			}
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			
			var lineHeight = Tile.Font.MeasureString("!g").Y;

			var height = Rectangle.Height * Tile.Size - Tile.Size;
			var y = height - m_lines.Count*lineHeight;

			foreach (var textLine in m_lines)
			{
				if(y>0)
				{
					DrawLine(textLine, Tile.Font, Color.Gray, _spriteBatch, Tile.Size, Rectangle.Top*Tile.Size + y);
				}
				y+=lineHeight;

			}


//            var str = @"Старик рыбачил один на своей лодке в Гольфстриме. Вот уже восемьдесят четыре дня он ходил в море и не поймал ни одной рыбы. Первые сорок дней с ним был мальчик. Но день за днем не приносил улова, и родители сказали мальчику, что старик теперь уже явно salao, то есть «самый что ни на есть невезучий», и велели ходить в море на другой лодке, которая действительно привезла три хорошие рыбы в первую же неделю. Мальчику тяжело было смотреть, как старик каждый день возвращается ни с чем, и он выходил на берег, чтобы помочь ему отнести домой снасти или багор, гарпун и обернутый вокруг мачты парус. Парус был весь в заплатах из мешковины и, свернутый, напоминал знамя наголову разбитого полка.
//Старик был худ и изможден, затылок его прорезали глубокие морщины, а щеки были покрыты коричневыми пятнами неопасного кожного рака, который вызывают солнечные лучи, отраженные гладью тропического моря. Пятна спускались по щекам до самой шеи, на руках виднелись глубокие шрамы, прорезанные бечевой, когда он вытаскивал крупную рыбу. Однако свежих шрамов не было. Они были стары, как трещины в давно уже безводной пустыне.
//Все у него было старое, кроме глаз, а глаза были цветом похожи на море, веселые глаза человека, который не сдается.
//- Сантьяго, - сказал ему мальчик, когда они вдвоем поднимались по дороге от берега, где стояла на причале лодка, - теперь я опять могу пойти с тобой в море. Мы уже заработали немного денег.
//Старик научил мальчика рыбачить, и мальчик его любил.
//- Нет, - сказал старик, - ты попал на счастливую лодку. Оставайся на ней.
//- А помнишь, один раз ты ходил в море целых восемьдесят семь дней и ничего не поймал, а потом мы три недели кряду каждый день привозили по большой рыбе.
//- Помню, - сказал старик. - Я знаю, ты ушел от меня не потому, что не верил.
//- Меня заставил отец, а я еще мальчик и должен слушаться.
//- Знаю, - сказал старик. - Как же иначе.
//- Он-то не очень верит.
//- Да, - сказал старик. - А вот мы верим. Правда?";

//            var highlights = new Dictionary<string, Color> { { "Старик", Color.Crimson }, { "он", Color.Green } };
//            DrawText(new TextPortion(str, highlights), _spriteBatch, Tile.Font, Color.White, 0);

			_spriteBatch.End();
		}
	}
}
