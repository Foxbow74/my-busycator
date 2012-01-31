using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		LockType LockType { get; }
		EActResults Open(Creature _creature, MapCell _mapCell, bool _silence);
	}

	internal interface ICanbeClosed
	{
		LockType LockType { get; }
		EActResults Close(Creature _creature, MapCell _mapCell, bool _silence);
	}
}