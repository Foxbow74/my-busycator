using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Essences.Things
{
	internal class ClosedDoor : Thing, ICanbeOpened
	{
		private ELockType m_eLockType = ELockType.SIMPLE;

		public ClosedDoor(Material _material) : base("дверь".AsNoun(ESex.FEMALE, false), _material) { Sex = ESex.FEMALE; }

        public override int TileIndex { get { return 8; } }

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, LiveMapCell _liveMapCell)
		{
			var door = new OpenDoor(Material);
			door.SetLockType(m_eLockType);
			_liveMapCell.Thing = door;

			if (_creature.IsAvatar)
			{
				MessageManager.SendXMessage(this, new XMessage(EALTurnMessage.CREATURE_OPENS_IT, _creature, this));
			}
			return EActResults.DONE;
		}

		public virtual ELockType ELockType { get { return m_eLockType; } }

		#endregion

		internal void SetLockType(ELockType _eLockType) { m_eLockType = _eLockType; }
	}
}