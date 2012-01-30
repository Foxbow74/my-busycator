using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		EActResults Open(Creature _creature, MapCell _mapCell, bool _silence);
		LockType LockType { get; }
	}

	internal interface ICanbeClosed
	{
		EActResults Close(Creature _creature, MapCell _mapCell, bool _silence);
		LockType LockType { get; }
	}
}