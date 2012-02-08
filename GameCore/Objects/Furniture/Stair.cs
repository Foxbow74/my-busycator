using GameCore.Mapping.Layers;

namespace GameCore.Objects.Furniture
{
	abstract class Stair : Thing
	{
		public WorldLayer LeadToLayer { get; protected  set; }
	}
}