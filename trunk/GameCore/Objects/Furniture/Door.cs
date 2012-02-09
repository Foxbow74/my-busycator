using System;
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

		public override string Name
		{
			get { return "дверь"; }
		}

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			_mapCell.RemoveFurnitureFromBlock();
			var door = new OpenDoor();
			door.SetLockType(m_eLockType);
			_mapCell.AddObjectToBlock(door);

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

		public virtual EActResults Close(Creature _creature, MapCell _mapCell, bool _silence)
		{
			throw new NotImplementedException();
		}
	}
}