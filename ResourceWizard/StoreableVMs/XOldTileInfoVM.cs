using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClientCommonWpf;
using GameCore;
using GameCore.Storage;
using GameUi;
using ResourceWizard.VMs;
using XTransport;
using XTransport.WPF;

namespace ResourceWizard.StoreableVMs
{
	class XOldTileInfoVM : ClientXObjectVM<EStoreKind>
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.TILE_INFO; }
		}

		public XOldTileInfoVM()
		{
			TextureVM = new TextureVM(){Texture = Texture, CursorX = X * Constants.TILE_SIZE, CursorY = Y * Constants.TILE_SIZE};
		}

		[X("TEXTURE")]private readonly IXValue<int> m_eTexture;
		[X("ETile")]private IXValue<int> m_eTile;
		[X("X")]private IXValue<int> m_x;
		[X("Y")]private IXValue<int> m_y;
		[X("Color")]private IXValue<string> m_color;

		public int X { get { return m_x.Value; } set { m_x.Value = value; } }

		public int Y { get { return m_y.Value; } set { m_y.Value = value; } }

		public FColor Color { get { return FColor.Parse(m_color.Value); } set { m_color.Value = value.ToShortText(); } }

		public ETiles Tile { get { return (ETiles)m_eTile.Value; } set { m_eTile.Value = (int)value; } }

		public ETextureSet Texture
		{
			get { return (ETextureSet) m_eTexture.Value; }
			set
			{
				m_eTexture.Value = (int) value;
			}
		}

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_color, () => Color);
			BindProperty(m_eTexture, () => Texture);
			BindProperty(m_eTile, () => Tile);
			BindProperty(m_x, () => X);
			BindProperty(m_y, () => Y);
		}

		public TextureVM TextureVM { get; private set; }
	}
}
