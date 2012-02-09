using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects.Furniture
{
	internal class OpenDoor : Furniture, ICanbeClosed
	{
		private ELockType m_eLockType = ELockType.SIMPLE;

		public override ETiles Tile
		{
			get { return ETiles.OPEN_DOOR; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}

		#region ICanbeClosed Members

		public EActResults Close(Creature _creature, MapCell _mapCell, bool _silence)
		{
			_mapCell.RemoveFurnitureFromBlock();
			var door = new Door();
			door.SetLockType(m_eLockType);
			_mapCell.AddObjectToBlock(door);
			if (!_silence) MessageManager.SendMessage(this, this.GetName(_creature) + " закрыта.");
			return EActResults.DONE;
		}

		public ELockType ELockType
		{
			get { return ELockType.OPEN; }
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