using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	internal abstract class Stair : Furniture
	{
		public WorldLayer LeadToLayer { get; protected set; }
	}
}