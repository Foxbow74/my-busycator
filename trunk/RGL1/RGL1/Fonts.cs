#region

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace RGL1
{
	public static class Fonts
	{
		public static SpriteFont Font { get; private set; }
		public static SpriteFont SmallFont { get; private set; }

		public static void Init(ContentManager _content)
		{
			Font = _content.Load<SpriteFont>("micross");
			SmallFont = _content.Load<SpriteFont>("consolas");
		}
	}
}