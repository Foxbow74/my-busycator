using System;
using RGL1.Messages;
using Common;

namespace GameCore
{
	public class World
	{
		static World()
		{
			Rnd = new Random(1);
		}

		public World()
		{
			Avatar = new Avatar();
			Map = new Map();
		}

		/// <summary>
		/// Ход в игре с точки зрения игрока
		/// Так как скорости не однородны, с точки зрения медленных или быстрых монстров выглядит иначе
		/// </summary>
		public long Turn { get; private set; }

		public Map Map { get; private set; }

		public Avatar Avatar { get; private set; }

		public static Random Rnd{ get; private set; }

		public void CommandReceived(ECommands _command)
		{
			
		}

		public void MoveCommandReceived(int _dx, int _dy)
		{
			var newX = Avatar.Point.X + _dx;
			var newY = Avatar.Point.Y + _dy;

			var terrain = Map.GetTerrain(newX, newY);

			var attr = terrain.GetAttribute<ETerrains, TerrainAttribute>();

			if (attr.IsPassable)
			{
				Avatar.Point.X = newX;
				Avatar.Point.Y = newY;
				MessageManager.SendMessage(this, new TextMessage(EMessageType.DEBUG, attr.DisplayName, null));
			}
			else
			{
				MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, "неа, " + attr.DisplayName, null));
			}
			MessageManager.SendMessage(this, new TurnMessage());

		}
	}
}