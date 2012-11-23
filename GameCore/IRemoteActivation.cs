using GameCore.Creatures;
using GameCore.Misc;

namespace GameCore
{
	public interface IRemoteActivation
	{
		uint MechanismId { get; }
		void RemoteActivation(Creature _creature, Point _worldCoords);
	}
}