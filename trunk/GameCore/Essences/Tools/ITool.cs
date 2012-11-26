using GameCore.Acts;
using GameCore.Creatures;

namespace GameCore.Essences.Tools
{
	public interface ITool
	{
		EActResults UseTool(Intelligent _intelligent);
	}
}