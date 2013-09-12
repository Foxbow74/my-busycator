using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Essences.Tools;

namespace MagickSetting.Items.Tools
{
	class Torch : AbstractTorch
	{
		public Torch(Material _material)
			: base(EALNouns.Torch, _material, 30, FColor.Orange)
		{
		}
	}
}
