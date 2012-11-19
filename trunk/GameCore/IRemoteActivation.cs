using GameCore.Creatures;
using GameCore.Misc;
using UnsafeUtils;

namespace GameCore
{
	public interface IRemoteActivation
	{
		uint MechanismId { get; }
		void RemoteActivation(Creature _creature, Point _worldCoords);
	}
}