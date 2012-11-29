using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using GameCore;
using GameCore.Essences;
using GameCore.Misc;
using GameCore.Storage;
using MagickSetting;
using ResourceWizard.Properties;
using ResourceWizard.StoreableVMs;
using Image = OpenTKUi.Image;

namespace ResourceWizard
{
	class Manager
	{
		private static Manager m_instance;
		public readonly ColorDialog COLOR_DIALOG = new ColorDialog { FullOpen = true };

		readonly XResourceServer m_resourceSrv = new XResourceServer();
		readonly XClient m_resourceCli = new XClient();
		
		readonly Dictionary<ETextureSet, Image> m_textures = new Dictionary<ETextureSet, Image>();
		readonly Dictionary<ETextureSet, Dictionary<Tuple<int, int, FColor, bool, bool>, Bitmap>> m_tiles = new Dictionary<ETextureSet, Dictionary<Tuple<int, int, FColor, bool, bool>, Bitmap>>();
		readonly Dictionary<ETextureSet, Image> m_ttextures = new Dictionary<ETextureSet, Image>();
		private IEnumerable<Essence> m_allEssences;

		private Manager()
		{
			MagicSettingProvider.Init();

			m_ttextures.Add(ETextureSet.GP, new Image(Resources.gold_plated_16x16, false, false));
			m_ttextures.Add(ETextureSet.HM, new Image(Resources.aq, false, false));
			m_ttextures.Add(ETextureSet.NH, new Image(Resources.nethack, false, false));
			m_ttextures.Add(ETextureSet.PH, new Image(Resources.Phoebus_16x16, false, false));
			m_ttextures.Add(ETextureSet.RB1, new Image(Resources.RantingRodent_Brick_01, false, false));
			m_ttextures.Add(ETextureSet.RB2, new Image(Resources.RantingRodent_Brick_02, false, false));
			m_ttextures.Add(ETextureSet.RN1, new Image(Resources.RantingRodent_Natural_01, false, false));
			m_ttextures.Add(ETextureSet.RN2, new Image(Resources.RantingRodent_Natural_02, false, false));
			m_ttextures.Add(ETextureSet.RJ, new Image(Resources.redjack15v, false, false));
			m_ttextures.Add(ETextureSet.U4, new Image(Resources.Ultima4, false, false));
			m_ttextures.Add(ETextureSet.U5, new Image(Resources.Ultima5, false, false));
			m_ttextures.Add(ETextureSet.WC_SG, new Image(Resources.summergrass, false, false));
			m_ttextures.Add(ETextureSet.WC_SW, new Image(Resources.summerwater, false, false));
			m_ttextures.Add(ETextureSet.WC_WS, new Image(Resources.wintersnow, false, false));
			m_ttextures.Add(ETextureSet.dg_armor32, new Image(Scale(Resources.dg_armor32), false, false));
			m_ttextures.Add(ETextureSet.dg_monster1, new Image(Scale(Resources.dg_monster132), false, false));
			m_ttextures.Add(ETextureSet.dg_monster2, new Image(Scale(Resources.dg_monster232), false, false));
			m_ttextures.Add(ETextureSet.dg_monster3, new Image(Scale(Resources.dg_monster332), false, false));
			m_ttextures.Add(ETextureSet.dg_monster4, new Image(Scale(Resources.dg_monster432), false, false));
			m_ttextures.Add(ETextureSet.dg_monster5, new Image(Scale(Resources.dg_monster532), false, false));
			m_ttextures.Add(ETextureSet.dg_monster6, new Image(Scale(Resources.dg_monster632), false, false));
			m_ttextures.Add(ETextureSet.dg_monster7, new Image(Scale(Resources.dg_monster732), false, false));
			m_ttextures.Add(ETextureSet.dg_misc32, new Image(Scale(Resources.dg_misc32), false, false));
			m_ttextures.Add(ETextureSet.dg_potions32, new Image(Scale(Resources.dg_potions32), false, false));
			m_ttextures.Add(ETextureSet.dg_undead32, new Image(Scale(Resources.dg_undead32), false, false));
			m_ttextures.Add(ETextureSet.dg_wands32, new Image(Scale(Resources.dg_wands32), false, false));
			m_ttextures.Add(ETextureSet.dg_weapons32, new Image(Scale(Resources.dg_weapons32), false, false));

			m_ttextures.Add(ETextureSet.dg_classm32, new Image(Scale(Resources.dg_classm32), false, false));
			m_ttextures.Add(ETextureSet.dg_dragon32, new Image(Scale(Resources.dg_dragon32), false, false));
			m_ttextures.Add(ETextureSet.dg_dungeon32, new Image(Scale(Resources.dg_dungeon32), false, false));
			m_ttextures.Add(ETextureSet.dg_edging132, new Image(Scale(Resources.dg_edging132), false, false));
			m_ttextures.Add(ETextureSet.dg_edging232, new Image(Scale(Resources.dg_edging232), false, false));
			m_ttextures.Add(ETextureSet.dg_edging332, new Image(Scale(Resources.dg_edging332), false, false));
			m_ttextures.Add(ETextureSet.dg_effects32, new Image(Scale(Resources.dg_effects32), false, false));
			m_ttextures.Add(ETextureSet.dg_food32, new Image(Scale(Resources.dg_food32), false, false));
			m_ttextures.Add(ETextureSet.dg_extra132, new Image(Scale(Resources.dg_extra132), false, false));
			m_ttextures.Add(ETextureSet.dg_grounds32, new Image(Scale(Resources.dg_grounds32), false, false));
			m_ttextures.Add(ETextureSet.dg_humans32, new Image(Scale(Resources.dg_humans32), false, false));
			m_ttextures.Add(ETextureSet.townactions, new Image(Scale(Resources.townactions), false, false));
			m_ttextures.Add(ETextureSet.dg_people32, new Image(Scale(Resources.dg_people32), false, false));
		}

		public XResourceRootVM XRoot
		{
			get
			{
				return m_resourceCli.GetRoot<XResourceRootVM>();
			}
		}

		public static Manager Instance
		{
			get { return m_instance ?? (m_instance = new Manager()); }
		}

		public Dispatcher Dispatcher { get; set; }

		public XTileInfoVM TileBuffer { get; set; }

		public bool HasChanges
	    {
	        get
	        {
	            //return true;
	            return XRoot.IsDirty;
	        }
	    }

		public App Application { get; set; }

		public XClient XClient
		{
			get { return m_resourceCli; }
		}


		public Bitmap this[ETextureSet _set, bool _isTerrain]
		{
			get
			{
				Image value;
				if (_isTerrain)
				{
					if (!m_ttextures.TryGetValue(_set, out value))
					{
						var scale = 1;
						Bitmap bmp;
						switch (_set)
						{
							case ETextureSet.RJ:
								bmp = Resources.redjack15v;
								break;
							case ETextureSet.RB1:
								bmp = Resources.RantingRodent_Brick_01;
								break;
							case ETextureSet.RB2:
								bmp = Resources.RantingRodent_Brick_02;
								break;
							case ETextureSet.RN1:
								bmp = Resources.RantingRodent_Natural_01;
								break;
							case ETextureSet.RN2:
								bmp = Resources.RantingRodent_Natural_02;
								break;
							case ETextureSet.GP:
								bmp = Resources.gold_plated_16x16;
								break;
							case ETextureSet.NH:
								bmp = Resources.nethack;
								break;
							case ETextureSet.HM:
								bmp = Resources.aq;
								break;
							case ETextureSet.PH:
								bmp = Resources.Phoebus_16x16;
								break;
							case ETextureSet.U4:
								bmp = Resources.Ultima4;
								break;
							case ETextureSet.U5:
								bmp = Resources.Ultima5;
								break;
							case ETextureSet.WC_WS:
								bmp = Resources.wintersnow;
								break;
							case ETextureSet.WC_SG:
								bmp = Resources.summergrass;
								break;
							case ETextureSet.WC_SW:
								bmp = Resources.summerwater;
								break;
							case ETextureSet.dg_armor32:
								bmp = Resources.dg_armor32;
								scale = 2;
								break;
							case ETextureSet.dg_monster1:
								bmp = Resources.dg_monster132;
								scale = 2;
								break;
							case ETextureSet.dg_monster2:
								bmp = Resources.dg_monster232;
								scale = 2;
								break;
							case ETextureSet.dg_monster3:
								bmp = Resources.dg_monster332;
								scale = 2;
								break;
							case ETextureSet.dg_monster4:
								bmp = Resources.dg_monster432;
								scale = 2;
								break;
							case ETextureSet.dg_monster5:
								bmp = Resources.dg_monster532;
								scale = 2;
								break;
							case ETextureSet.dg_monster6:
								bmp = Resources.dg_monster632;
								scale = 2;
								break;
							case ETextureSet.dg_monster7:
								bmp = Resources.dg_monster732;
								scale = 2;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
						if(scale==2)
						{
							bmp = Scale(bmp);
						}

						value = new Image(bmp, false, false);

						m_ttextures[_set] = value;
					}
				}
				else
				{
					if (!m_textures.TryGetValue(_set, out value))
					{
						var bmp = this[_set, true];
						value = new Image(bmp, true, false);
						m_textures[_set] = value;
					}
				}

				return value.Bitmap;
			}
		}

		private static Bitmap Scale(Bitmap _bmp)
		{
			var value2 = new Image(_bmp, true, false);
			var bmp2 = new Bitmap(_bmp.Width/2, _bmp.Height/2, PixelFormat.Format32bppPArgb);
			using (var gr = Graphics.FromImage(bmp2))
			{
				gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gr.DrawImage(value2.Bitmap, new Rectangle(0, 0, bmp2.Width, bmp2.Height), new Rectangle(0, 0, _bmp.Width, _bmp.Height),
				             GraphicsUnit.Pixel);
			}
			return bmp2;
		}

		public Bitmap this[XTileInfoVM _tile, FColor _fColor, bool _removeTransparency, bool _grayScale]
	    {
	        get
	        {
                if (_tile == null) return null;
	            return this[_tile.Texture, _tile.X, _tile.Y, _fColor, _removeTransparency, _grayScale, _tile.Parent is XTerrainSetVM];
	        }
	    }

		public Bitmap this[ETextureSet _texture, int _x, int _y, FColor _fColor, bool _removeTransparency, bool _grayScale, bool _isTerrain]
		{
			get
			{
                Dictionary<Tuple<int, int, FColor, bool, bool>, Bitmap> dictionary;
				if (!m_tiles.TryGetValue(_texture, out dictionary))
				{
                    dictionary = new Dictionary<Tuple<int, int, FColor, bool, bool>, Bitmap>();
					m_tiles[_texture] = dictionary;
				}
				Bitmap bitmap;
                var key = new Tuple<int, int, FColor, bool, bool>(_x, _y, _fColor, _removeTransparency,_grayScale);
				if (!dictionary.TryGetValue(key, out bitmap))
				{
					var txtr = this[_texture, _isTerrain];
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
                        if(_grayScale)
                        {
                            fcolor = fcolor.ToGrayScale();
                        }
						var result = fcolor.Multiply(_fColor).GetColor();//.Multiply(255);
						if (_removeTransparency)
						{
							result.A = 255;
						}
						bitmap.SetPixel(point.X, point.Y, Color.FromArgb(result.A, result.R, result.G, result.B));
					}

					dictionary.Add(key, bitmap);
				}
				return bitmap;
			}
		}

		public IEnumerable<Essence> GetEssences(ETileset _tileset)
		{
			if(m_allEssences==null)
			{
				m_allEssences = EssenceHelper.AllEssences;
			}
			return m_allEssences.Where(_thing => _thing.Tileset==_tileset);
		}

		public IEnumerable<Essence> GetEssences(ETileset _tileset, int _index)
		{
			return GetEssences(_tileset).Where(_thing => _thing.TileIndex==_index);
		}

		public void Save()
		{
			PackTiles();
			XRoot.BeforeSave();

			if(XRoot.NickInfos.Count==0)
			{
				var males = new XNicksInfoVM();
				var females = new XNicksInfoVM();
				XRoot.NickInfos.Add(males);
				XRoot.NickInfos.Add(females);

				males.Sex = ESex.MALE;
				males.Nicks = Resources.malenicks;

				females.Sex = ESex.FEMALE;
				females.Nicks = Resources.femalenicks;
			}
			m_resourceCli.Save(XRoot);
		}


		public void Shrink()
		{
			XRoot.EssenceProviders.Clear();

			var resourceEssenceGenerator = new XResourceEssenceGenerator(XRoot);
			if(resourceEssenceGenerator.Generate().Count()<2)
			{
				throw new ApplicationException("Хде они?");
			}
			m_resourceCli.Save(XRoot);
			m_resourceSrv.Shrink();
			Application.MainWindow.Close();
		}


		private void PackTiles()
		{
			var tileInfos = XRoot.TerrainSets.Cast<XAbstractTileSetVM>().Union(XRoot.TileSets).SelectMany(_vm => _vm.Children).GroupBy(_vm => Tuple.Create(_vm.Texture, _vm.X, _vm.Y, _vm.RemoveTransparency, _vm.GrayScale)).ToList();
			var size = (int)Math.Sqrt(tileInfos.Count) + 1;
			var sizeInPixels = size * Constants.TILE_SIZE;

			var begin = 16;
			for (var i = 1; ; ++i)
			{
				if (sizeInPixels <= begin)
				{
					sizeInPixels = begin;
					break;
				}
				begin *= 2;
			}

			var bmp = new Bitmap(sizeInPixels, sizeInPixels, PixelFormat.Format32bppPArgb);
			var srcRect = new Rectangle(0, 0, Constants.TILE_SIZE, Constants.TILE_SIZE);
			using (var gr = Graphics.FromImage(bmp))
			{
				var perRow = sizeInPixels / 16;
				for (var index = 0; index < tileInfos.Count; index++)
				{
					var grouping = tileInfos[index];
					var x = (index + 1) % perRow;
					var y = (index + 1) / perRow;

					var isNone = false;
					var xTileInfoVM = grouping.First();

					var prnt = xTileInfoVM.Parent;
					if(prnt is XTileSetVM)
					{
						isNone = ((XTileSetVM)prnt).Key == ETileset.NONE;
					}
					else if(prnt is XTerrainSetVM)
					{
						isNone = ((XTerrainSetVM)prnt).Key == ETerrains.NONE;
					}

					if(isNone)
					{
						x = 0;
						y = 0;
					}
					foreach (var vm in grouping)
					{
						vm.CX = x;
						vm.CY = y;
					}

					if(isNone) continue;

					var dstRect = new Rectangle(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                    
					var key = grouping.Key;

					gr.DrawImage(this[key.Item1, key.Item2, key.Item3, FColor.White.UpdateAlfa(xTileInfoVM.Color.A), key.Item4, key.Item5, xTileInfoVM.Parent is XTerrainSetVM], dstRect, srcRect, GraphicsUnit.Pixel);
				}
			}
			bmp.Save(Constants.RESOURCES_PNG_FILE, ImageFormat.Png);
		}
	}
}
