using System.Collections.Generic;
using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RGL1.Messages;

namespace RGL1.UIBlocks
{
	class MessageBlock:UIBlock
	{
		private readonly List<Message> m_history = new List<Message>();

		public MessageBlock(Rectangle _rectangle)
			: base(_rectangle, Frame.GoldFrame, Color.Yellow)
		{
			MessageManager.NewMessage += MessageManagerNewMessage;
		}

		void MessageManagerNewMessage(object _sender, Message _message)
		{
			m_history.Add(_message);
		}

		public override void DrawContent(SpriteBatch _spriteBatch)
		{
			_spriteBatch.Begin();
			var str = @"Игра будет называться Dwarf Defense. Cмесь Tower Defense (оборона от волн монстров), Dwarf Fortress (строительство укреплений и производство предметов) и все это в виде ala RogueLike (ascii - графика).Описание: вы управляете дфарфом-одиночкой, который прибывает на новое место, начинает строительство своего дома, добывать полезные ископаемые, производить различные предметы и строить оборону (башни, ловушки, големов и т.п.). Затем на его крепость начинают нападать волны различной нечисти (орки, гоблины, драконы, нежить и т.п.), с которыми он справляется в рукопашную и при помощи своей обороны. В том месте, где вы высаживаетесь, так же содержатся подземные пещеры, населенные своими обитателями и заброшенные подземные комплексы/лабиринты.
						Игра будет называться Dwarf Defense. Cмесь Tower Defense (оборона от волн монстров), Dwarf Fortress (строительство укреплений и производство предметов) и все это в виде ala RogueLike (ascii - графика).Описание: вы управляете дфарфом-одиночкой, который прибывает на новое место, начинает строительство своего дома, добывать полезные ископаемые, производить различные предметы и строить оборону (башни, ловушки, големов и т.п.). Затем на его крепость начинают нападать волны различной нечисти (орки, гоблины, драконы, нежить и т.п.), с которыми он справляется в рукопашную и при помощи своей обороны. В том месте, где вы высаживаетесь, так же содержатся подземные пещеры, населенные своими обитателями и заброшенные подземные комплексы/лабиринты.";

			var highlights = new Dictionary<string, Color> { { "Игра", Color.Crimson }, { "вы", Color.Green } };
			DrawText(new TextPortion(str, highlights), _spriteBatch, Tile.Font, Color.White);

			_spriteBatch.End();
		}
	}
}
