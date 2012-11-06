using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using GameCore;
using GameCore.Misc;
using GameCore.Storage;
using GameUi;
using ResourceWizard.StoreableVMs;

namespace ResourceWizard
{
	class Manager
	{
		public readonly ColorDialog ColorDialog = new ColorDialog { FullOpen = true };

		readonly XResourceServer m_resourceSrv = new XResourceServer();
		readonly XClient m_resourceCli = new XClient();

		private static Manager m_instance;

		private Manager()
		{
		}

		public XResourceRootVM XRoot
		{
			get
			{
				return m_resourceCli.GetRoot<XResourceRootVM>();
			}
		}

		public void Save() { World.SaveResources(); }

		public static Manager Instance
		{
			get { return m_instance ?? (m_instance = new Manager()); }
		}

		public Dispatcher Dispatcher { get; set; }

		public XTileInfoVM TileBuffer { get; set; }

		readonly Dictionary<ETextureSet, OpenTKUi.Image> m_textures = new Dictionary<ETextureSet, OpenTKUi.Image>();
		readonly Dictionary<ETextureSet, Dictionary<Tuple<int, int, FColor, bool>, Bitmap>> m_tiles = new Dictionary<ETextureSet, Dictionary<Tuple<int, int, FColor, bool>, Bitmap>>();


		public Bitmap this[ETextureSet _set]
		{
			get
			{
				OpenTKUi.Image value;
				if(!m_textures.TryGetValue(_set, out value))
				{
					string fileName;
					switch (_set)
					{
						case ETextureSet.RJ:
							fileName = "Resources\\redjack15v.bmp";
							break;
						case ETextureSet.RB1:
							fileName = "Resources\\RantingRodent_Brick_01.bmp";
							break;
						case ETextureSet.RB2:
							fileName = "Resources\\RantingRodent_Brick_02.bmp";
							break;
						case ETextureSet.RN1:
							fileName = "Resources\\RantingRodent_Natural_01.bmp";
							break;
						case ETextureSet.RN2:
							fileName = "Resources\\RantingRodent_Natural_02.bmp";
							break;
						case ETextureSet.GP:
							fileName = "Resources\\gold_plated_16x16.bmp";
							break;
						case ETextureSet.NH:
							fileName = "Resources\\nethack.bmp";
							break;
						case ETextureSet.HM:
							fileName = "Resources\\aq.png";
							break;
						case ETextureSet.PH:
							fileName = "Resources\\Phoebus_16x16.png";
							break;
						case ETextureSet.U4:
							fileName = "Resources\\Ultima4.png";
							break;
						case ETextureSet.U5:
							fileName = "Resources\\Ultima5.png";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					value = new OpenTKUi.Image(new Bitmap(fileName), true, false);
					m_textures[_set] = value;
				}
				return value.Bitmap;
			}
		}

		public Bitmap this[ETextureSet _texture, int _x, int _y, Color _color, FColor _fColor, bool _removeTransparency]
		{
			get
			{
				Dictionary<Tuple<int, int, FColor, bool>, Bitmap> dictionary;
				if (!m_tiles.TryGetValue(_texture, out dictionary))
				{
					dictionary = new Dictionary<Tuple<int, int, FColor, bool>, Bitmap>();
					m_tiles[_texture] = dictionary;
				}
				Bitmap bitmap;
				var key = new Tuple<int, int, FColor, bool>(_x, _y, _fColor, _removeTransparency);
				if (!dictionary.TryGetValue(key, out bitmap))
				{
					var txtr = this[_texture];
					bitmap = new Bitmap(Constants.TILE_SIZE, Constants.TILE_SIZE);
					using(var gr = Graphics.FromImage(bitmap))
					{
						gr.DrawImage(txtr, 0, 0, new Rectangle(Constants.TILE_SIZE * _x, Constants.TILE_SIZE * _y, Constants.TILE_SIZE, Constants.TILE_SIZE), GraphicsUnit.Pixel);
					}
					var transparent = txtr.GetPixel(0, 0);
					var rct = new Rct(0, 0, Constants.TILE_SIZE, Constants.TILE_SIZE);
					foreach (var point in rct.AllPoints)
					{
						var pixel = bitmap.GetPixel(point.X, point.Y);
						if (pixel == transparent) continue;
						var fcolor = new FColor(pixel.A, pixel.R, pixel.G, pixel.B).Multiply(1f / 255f);
						var result = fcolor.Multiply(_fColor).Multiply(255);
						if (_removeTransparency)
						{
							result.A = 255;
						}
						bitmap.SetPixel(point.X, point.Y, Color.FromArgb((int)result.A, (int)result.R, (int)result.G, (int)result.B));
					}

					dictionary.Add(key, bitmap);
				}
				return bitmap;
			}
		}
	}
}
