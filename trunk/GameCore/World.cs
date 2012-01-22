using System;
using GameCore.Objects;
using RGL1.Messages;
using Common;
using Object = GameCore.Objects.Object;

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

			var mapCell = Map.GetMapCell(newX, newY);

			var attr = mapCell.Terrain.GetAttribute<ETerrains, TerrainAttribute>();

			if (attr.IsPassable>0)
			{
				Avatar.Point.X = newX;
				Avatar.Point.Y = newY;

				var o = mapCell.Object;
				if (o == null)
				{
					MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, attr.DisplayName, null));
				}
				else
				{

					if(o is FakeItem)
					{
						o = mapCell.ResolveFakeItem((FakeItem)o);
					}
					MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, o.Name, null));
				}
			}
			else
			{
				MessageManager.SendMessage(this, new TextMessage(EMessageType.INFO, "неа, " + attr.DisplayName, null));
			}
			MessageManager.SendMessage(this, new TurnMessage());

		}
	}
}