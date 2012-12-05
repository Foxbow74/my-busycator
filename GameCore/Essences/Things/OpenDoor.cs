using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Essences.Things
{
	internal class OpenDoor : Thing, ICanbeClosed
	{
		private ELockType m_eLockType = ELockType.SIMPLE;

		public OpenDoor(Material _material) : base(EALNouns.Door, _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 9; } }

		#region ICanbeClosed Members

		public EActResults Close(Creature _creature, LiveMapCell _liveMapCell)
		{
			var door = new ClosedDoor(Material);
			door.SetLockType(m_eLockType);
			_liveMapCell.Thing = door;
			if (_creature.IsAvatar)
			{
				MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURE_CLOSES_IT, _creature, this));
				//MessageManager.SendMessage(this, this[EPadej.IMEN] + " закрыта.");
			}
			return EActResults.DONE;
		}

		public ELockType ELockType { get { return ELockType.OPEN; } }

		#endregion

		internal void SetLockType(ELockType _eLockType) { m_eLockType = _eLockType; }
	}
}