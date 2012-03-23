using System.Linq;
using GameCore.Acts;
using GameCore.Acts.Movement;
using GameCore.Mapping.Layers;
using GameCore.Misc;
using GameCore;

namespace GameCore.Creatures
{
	public class Monster : Creature
	{
		public Monster(WorldLayer _layer)
			: base(_layer, 100)
		{
		}

		public override ETiles Tile
		{
			get { return ETiles.MONSTER; }
		}

		public override string Name
		{
			get { return "существо" + Nn; }
		}

		public override void Resolve(Creature _creature)
		{
		}

		private int m_skip = 0;

		public override EThinkingResult Thinking()
		{
			if(m_skip>0)
			{
				m_skip--;
				AddActToPool(new WaitAct());
				return EThinkingResult.NORMAL;
			}
			var destLiveCell = World.TheWorld.Avatar[0, 0];
			var myLiveCell = this[0, 0];
			if (destLiveCell.InRoom == myLiveCell.InRoom)
			{
				AddActToPool(new WaitAct());
				return EThinkingResult.NORMAL;
			}

			var path = World.TheWorld.LiveMap.PathFinder.FindPath(this, destLiveCell.PathMapCoords + Point.NearestDPoints.ToArray().RandomItem(World.Rnd));
			if (path != null)
			{
				AddActToPool(new MoveToAct(this, path));
				return EThinkingResult.NORMAL;
			}
			else
			{
				m_skip = 6;
			}

			AddActToPool(new MoveAct(), new Point(World.Rnd.Next(3) - 1, World.Rnd.Next(3) - 1));
			return EThinkingResult.NORMAL;
		}
	}
}