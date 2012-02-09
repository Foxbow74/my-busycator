using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Objects.Furniture;

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		ELockType ELockType { get; }
		EActResults Open(Creature _creature, MapCell _mapCell, bool _silence);
	}

	internal interface ICanbeClosed
	{
		ELockType ELockType { get; }
		EActResults Close(Creature _creature, MapCell _mapCell, bool _silence);
	}
}