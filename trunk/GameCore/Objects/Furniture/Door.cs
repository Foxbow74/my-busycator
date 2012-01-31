using System;
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects.Furniture
{
	public class Door : Thing, ICanbeOpened
	{
		private LockType m_lockType = LockType.SIMPLE;

		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE; }
		}

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
			_mapCell.RemoveObjectFromBlock();
			var door = new OpenDoor();
			door.SetLockType(m_lockType);
			_mapCell.AddObjectFromBlock(door);

			if (!_silence) MessageManager.SendMessage(this, Name + " открыта.");
			return EActResults.DONE;
		}

		public virtual LockType LockType
		{
			get { return m_lockType; }
		}

		#endregion

		public override void Resolve(Creature _creature)
		{
		}

		internal void SetLockType(LockType _lockType)
		{
			m_lockType = _lockType;
		}

		public virtual EActResults Close(Creature _creature, MapCell _mapCell, bool _silence)
		{
			throw new NotImplementedException();
		}
	}
}