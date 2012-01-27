using System.Collections.Generic;
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

		public static WorldMessage Turn { get; private set; }
		public static WorldMessage AvatarTurn { get; private set; }
		public static WorldMessage AvatarMove { get; private set; } 

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

		public EType Type { get; private set; }
	}

	public abstract class AskMessage : Message
	{
		protected AskMessage(Act _act)
		{
			Act = _act;
		}
		public Act Act { get; private set; }
	}

	public class AskDirectionMessage : AskMessage
	{
		public AskDirectionMessage(Act _act) : base(_act)
		{
		}
	}

	public class AskSelectThingsMessage : AskMessage
	{
		public AskSelectThingsMessage(IEnumerable<ThingDescriptor> _items, Act _act)
			: base(_act)
		{
			ItemDescriptors = _items;
		}

		public IEnumerable<ThingDescriptor> ItemDescriptors { get; private set; }
	}
}