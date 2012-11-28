using System.Collections.Generic;
using GameCore;
using GameCore.Storage;
using MagickSetting.Items.Weapons;

namespace MagickSetting
{
	public static class EssenceGenerator
	{
		public static IEnumerable<IEssenceProvider> Generate(IEssenceProviderHelperGenerator _helperGenerator)
		{
			var sps = new SwordsProvider();

			var sp = _helperGenerator.Create<SwordsProvider>(sps.ProvierTypeId);
			sp.Name = "меч-зарубец";
			sp.TileIndex = 2;
			sp.IsArtifact = false;
			sp.Level = 4;
			sp.Materials = EMaterial.METAL;
			yield return sp;
		}
	}
}
