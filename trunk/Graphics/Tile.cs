using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics
{
	public enum TextureSet
	{
		REDJACK,
		RR_BRICK_01,
		RR_BRICK_02,
		RR_NATURAL_01,
		RR_NATURAL_02,
		GP_X16,
	}

	public class Tile
	{
		private static readonly Dictionary<TextureSet, Texture2D> m_textures = new Dictionary<TextureSet, Texture2D>();

		private readonly TextureSet m_set;
		public static SpriteFont Font { get; private set; }
	
		public static int Size = 16;

		public Color Color { get; private set; }

		public static void Init(ContentManager _content)
		{
			foreach (TextureSet set in Enum.GetValues(typeof(TextureSet)))
			{
				Texture2D texture;
				switch (set)
				{
					case TextureSet.REDJACK:
						texture = _content.Load<Texture2D>("redjack15v");
						break;
					case TextureSet.RR_BRICK_01:
						texture = _content.Load<Texture2D>("RantingRodent_Brick_01");
						break;
					case TextureSet.RR_BRICK_02:
						texture = _content.Load<Texture2D>("RantingRodent_Brick_02");
						break;
					case TextureSet.RR_NATURAL_01:
						texture = _content.Load<Texture2D>("RantingRodent_Natural_01");
						break;
					case TextureSet.RR_NATURAL_02:
						texture = _content.Load<Texture2D>("RantingRodent_Natural_02");
						break;
					case TextureSet.GP_X16:
						texture = _content.Load<Texture2D>("gold_plated_16x16");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				m_textures.Add(set, texture);
			}
			
			Font = _content.Load<SpriteFont>("micross");
		}


		public Rectangle Rectangle { get; private set; }

		public Texture2D Texture
		{
			get { return m_textures[m_set]; }
		}

		public Tile(TextureSet _set, int _x, int _y, Color _color)
		{
			m_set = _set;
			Color = _color;
			Rectangle = new Rectangle(_x * Size, _y * Size, Size, Size);
		}

		public Tile(int _x, int _y, Color _color)
		{
			m_set = TextureSet.REDJACK;
			Color = _color;
			Rectangle = new Rectangle(_x * Size, _y * Size, Size, Size);
		}

	}
}