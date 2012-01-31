using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects.Furniture
{
	public class OpenDoor : Thing, ICanbeClosed
	{
		private LockType m_lockType = LockType.SIMPLE;

		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE; }
		}

		public override float Opaque
		{
			get
			{
				return 0f;
			}
		}

		public override ETiles Tile
		{
			get { return ETiles.OPEN_DOOR; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		internal void SetLockType(LockType _lockType)
		{
			m_lockType = _lockType;
		}

		#region ICanbeClosed Members

		public EActResults Close(Creature _creature, MapCell _mapCell, bool _silence)
		{
			_mapCell.RemoveObjectFromBlock();
			var door = new Door();
			door.SetLockType(m_lockType);
			_mapCell.AddObjectFromBlock(door);
			if (!_silence) MessageManager.SendMessage(this, Name + " закрыта.");
			return EActResults.DONE;
		}

		public LockType LockType
		{
			get { return LockType.OPEN; }
		}

		#endregion
	}
}