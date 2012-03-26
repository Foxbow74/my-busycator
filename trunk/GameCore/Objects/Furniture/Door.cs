using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using RusLanguage;

namespace GameCore.Objects.Furniture
{
	internal class Door : FurnitureThing, ICanbeOpened
	{
		private ELockType m_eLockType = ELockType.SIMPLE;

		public Door(Material _material) : base(_material) { Sex = ESex.FEMALE; }

		public override ETiles Tile { get { return ETiles.DOOR; } }

		public override string Name { get { return "дверь"; } }

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, LiveMapCell _liveMapCell)
		{
			var door = new OpenDoor(Material);
			door.SetLockType(m_eLockType);
			_liveMapCell.Furniture = door;

			if (_creature.IsAvatar) MessageManager.SendMessage(this, this[EPadej.IMEN] + " открыта.");
			return EActResults.DONE;
		}

		public virtual ELockType ELockType { get { return m_eLockType; } }

		#endregion

		public override void Resolve(Creature _creature) { }

		internal void SetLockType(ELockType _eLockType) { m_eLockType = _eLockType; }
	}
}