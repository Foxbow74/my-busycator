using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Essences.Tools;

namespace MagickSetting.Items.Tools
{
	class Torch : AbstractTorch
	{
		public Torch(Material _material)
			: base(EALNouns.Torch, _material, 30, new FColor(1f, 1f, 0.9f, 0.5f))
		{
		}
	}
}
