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
			AVATAR_CHANGE_LAYER,
		}

		#endregion

		static WorldMessage()
		{
			Turn = new WorldMessage(EType.TURN);
			AvatarTurn = new WorldMessage(EType.AVATAR_TURN);
			AvatarMove = new WorldMessage(EType.AVATAR_MOVE);
			AvatarChangeLayer = new WorldMessage(EType.AVATAR_CHANGE_LAYER);
		}

		public WorldMessage(EType _type)
		{
			Type = _type;
		}

		public static WorldMessage Turn { get; private set; }
		public static WorldMessage AvatarTurn { get; private set; }
		public static WorldMessage AvatarMove { get; private set; }
		public static WorldMessage AvatarChangeLayer { get; private set; }

		public EType Type { get; private set; }
	}
}