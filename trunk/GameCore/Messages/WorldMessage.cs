using GameCore.Acts;
using GameCore.Objects;

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

		public static WorldMessage Turn = new WorldMessage(EType.TURN);
		public static WorldMessage AvatarTurn = new WorldMessage(EType.AVATAR_TURN);
		public static WorldMessage AvatarMove = new WorldMessage(EType.AVATAR_MOVE);

		public WorldMessage(EType _type)
		{
			Type = _type;
		}

		public EType Type { get; private set; }
	}

	public abstract class AskMessage : Message
	{
	}

	public class AskDirectionMessage : AskMessage
	{
	}

	public class SelectItemsMessage : AskMessage
	{
		public SelectItemsMessage(ItemsCollection _itemsCollection, Act _act)
		{
			ItemsCollection = _itemsCollection;
			Act = _act;
		}

		public ItemsCollection ItemsCollection { get; private set; }
		public Act Act { get; private set; }
	}
}