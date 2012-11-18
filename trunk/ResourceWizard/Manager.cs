using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using GameCore;
using GameCore.Misc;
using GameCore.Essences;
using GameCore.Storage;
using ResourceWizard.Properties;
using ResourceWizard.StoreableVMs;
using RusLanguage;
using Color = System.Drawing.Color;

namespace ResourceWizard
{
	class Manager
	{
		public readonly ColorDialog COLOR_DIALOG = new ColorDialog { FullOpen = true };

#pragma warning disable 169
		readonly XResourceServer m_resourceSrv = new XResourceServer();
#pragma warning restore 169

		readonly XClient m_resourceCli = new XClient();

		private static Manager m_instance;

		private Manager()
		{
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

		public XResourceRootVM XRoot
		{
			get
			{
				return m_resourceCli.GetRoot<XResourceRootVM>();
			}
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

           // m_resourceSrv.Shrink();
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
                    var prnt = grouping.First().Parent;
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

					//if (grouping.First().Parent is XTileSetVM)
					//{
					//    var ts = (XTileSetVM)grouping.First().Parent;
					//    if(ts.Key==ETileset.SPLATTERS)
					//    {
					//        gr.DrawImage(this[key.Item1, key.Item2, key.Item3, grouping.First().Color.GetFColor(), key.Item4, key.Item5], dstRect, srcRect, GraphicsUnit.Pixel);
					//        continue;
					//    }
					//}
					gr.DrawImage(this[key.Item1, key.Item2, key.Item3, FColor.White.UpdateAlfa(grouping.First().Color.A), key.Item4, key.Item5], dstRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            bmp.Save(Constants.RESOURCES_PNG_FILE, ImageFormat.Png);
	    }

	    public static Manager Instance
		{
			get { return m_instance ?? (m_instance = new Manager()); }
		}

		public Dispatcher Dispatcher { get; set; }

		public XTileInfoVM TileBuffer { get; set; }

		readonly Dictionary<ETextureSet, OpenTKUi.Image> m_textures = new Dictionary<ETextureSet, OpenTKUi.Image>();
        readonly Dictionary<ETextureSet, Dictionary<Tuple<int, int, FColor, bool, bool>, Bitmap>> m_tiles = new Dictionary<ETextureSet, Dictionary<Tuple<int, int, FColor, bool, bool>, Bitmap>>();
	    private IEnumerable<Essence> m_allEssences;
	    
	    public bool HasChanges
	    {
	        get
	        {
	            return true;
	            //return XRoot.IsDirty;
	        }
	    }


	    public Bitmap this[ETextureSet _set]
		{
			get
			{
				OpenTKUi.Image value;
				if(!m_textures.TryGetValue(_set, out value))
				{
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
						default:
							throw new ArgumentOutOfRangeException();
					}
					value = new OpenTKUi.Image(bmp, true, false);
					m_textures[_set] = value;
				}
				return value.Bitmap;
			}
		}

	    public Bitmap this[XTileInfoVM _tile, FColor _fColor, bool _removeTransparency, bool _grayScale]
	    {
	        get
	        {
                if (_tile == null) return null;
	            return this[_tile.Texture, _tile.X, _tile.Y, _fColor, _removeTransparency, _grayScale];
	        }
	    }

	    public Bitmap this[ETextureSet _texture, int _x, int _y, FColor _fColor, bool _removeTransparency, bool _grayScale]
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
	}
}
