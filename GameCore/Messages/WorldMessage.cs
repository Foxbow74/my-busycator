namespace GameCore.Messages
{
	public class WorldMessage
	{
		#region EType enum

		public enum EType
		{
			TURN,
			AVATAR_TURN,
			AVATAR_MOVE,
		}

		#endregion

		static WorldMessage()
		{
			Turn = new WorldMessage(EType.TURN);
			AvatarTurn = new WorldMessage(EType.AVATAR_TURN);
			AvatarMove = new WorldMessage(EType.AVATAR_MOVE);
		}

		public WorldMessage(EType _type)
		{
			Type = _type;
		}

		public static WorldMessage Turn { get; private set; }
		public static WorldMessage AvatarTurn { get; private set; }
		public static WorldMessage AvatarMove { get; private set; }

		public EType Type { get; private set; }
	}
}