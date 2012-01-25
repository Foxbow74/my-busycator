namespace GameCore.Messages
{
	public class WorldMessage
	{
		public static WorldMessage Turn = new WorldMessage(EType.TURN);
		public static WorldMessage AvatarTurn = new WorldMessage(EType.AVATAR_TURN);
		public static WorldMessage AvatarMove = new WorldMessage(EType.AVATAR_MOVE);

		public enum EType
		{
			TURN,
			AVATAR_TURN,
			AVATAR_MOVE,
		}

		public EType Type { get; private set; }

		public WorldMessage(EType _type)
		{
			Type = _type;
		}
	}
}