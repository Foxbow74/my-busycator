using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Essences.Tools;

namespace MagickSetting.Items.Tools
{
	class Torch : AbstractTorch
	{
		public Torch(Material _material)
			: base("факел".AsNoun(ESex.MALE, false), _material, 10, new FColor(2f, 1f, 0.9f, 0.5f))
		{
		}
	}
}
