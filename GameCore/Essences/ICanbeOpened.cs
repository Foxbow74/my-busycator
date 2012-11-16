using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Essences.Things;
using GameCore.Mapping;

namespace GameCore.Essences
{
	internal interface ICanbeOpened
	{
		ELockType ELockType { get; }
		EActResults Open(Creature _creature, LiveMapCell _liveMapCell);
	}

	internal interface ICanbeClosed
	{
		ELockType ELockType { get; }
		EActResults Close(Creature _creature, LiveMapCell _liveMapCell);
	}
}