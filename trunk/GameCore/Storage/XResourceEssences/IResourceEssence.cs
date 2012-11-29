using System;
using GameCore.Essences;

namespace GameCore.Storage.XResourceEssences
{
	public interface IResourceEssence
	{
		Essence Create(Material _material);
		Guid ProvierTypeId { get; }

		string Name { get; set; }
		int TileIndex { get; set; }
		int Level { get; set; }
		bool IsArtifact { get; set; }
		EMaterialType MaterialTypes { get; set; }
	}
}