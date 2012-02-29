using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects.Furniture
{
	internal class Door : Furniture, ICanbeOpened
	{
		private ELockType m_eLockType = ELockType.SIMPLE;

		public override ETiles Tile
		{
			get { return ETiles.DOOR; }
		}

		public override FColor LerpColor
		{
			get
			{
				return new FColor(0.5f,0f,1f,0f);
			}
		}

		public override string Name
		{
			get { return "дверь"; }
		}

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, LiveMapCell _liveMapCell, bool _silence)
		{
			var door = new OpenDoor();
			door.SetLockType(m_eLockType);
			_liveMapCell.Furniture = door;

			if (!_silence) MessageManager.SendMessage(this, this.GetName(_creature) + " открыта.");
			return EActResults.DONE;
		}

		public virtual ELockType ELockType
		{
			get { return m_eLockType; }
		}

		#endregion

		public override void Resolve(Creature _creature)
		{
		}

		internal void SetLockType(ELockType _eLockType)
		{
			m_eLockType = _eLockType;
		}
	}
}