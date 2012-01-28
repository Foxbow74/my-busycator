using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;

namespace GameCore.Objects
{
	internal interface ICanbeOpened
	{
		EActResults Open(Creature _creature, MapCell _mapCell, bool _silence);
		LockType LockType { get; }
	}

	public class Door : Thing, ICanbeOpened
	{
		private LockType m_lockType = LockType.SIMPLE;

		public override ETiles Tile
		{
			get { return ETiles.DOOR; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}

		public override void Resolve(Creature _creature)
		{
			
		}

		#region ICanbeOpened Members

		public EActResults Open(Creature _creature, MapCell _mapCell, bool _silence)
		{
			m_lockType = LockType.OPEN;
			if (!_silence) MessageManager.SendMessage(this, Name + " открыта.");
			return EActResults.DONE;
		}

		public LockType LockType
		{
			get { return m_lockType; }
		}

		#endregion
	}
}