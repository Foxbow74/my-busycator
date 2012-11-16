using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using RusLanguage;

namespace GameCore.Essences.Things
{
	internal class OpenDoor : Thing, ICanbeClosed
	{
		private ELockType m_eLockType = ELockType.SIMPLE;

		public OpenDoor(Material _material) : base(_material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 9; } }

		public override string Name { get { return "дверь"; } }

		#region ICanbeClosed Members

		public EActResults Close(Creature _creature, LiveMapCell _liveMapCell)
		{
			var door = new ClosedDoor(Material);
			door.SetLockType(m_eLockType);
			_liveMapCell.Thing = door;
			if (_creature.IsAvatar) MessageManager.SendMessage(this, this[EPadej.IMEN] + " закрыта.");
			return EActResults.DONE;
		}

		public ELockType ELockType { get { return ELockType.OPEN; } }

		#endregion

		public override void Resolve(Creature _creature) { }

		internal void SetLockType(ELockType _eLockType) { m_eLockType = _eLockType; }
	}
}