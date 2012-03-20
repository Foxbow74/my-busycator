using GameCore.Acts.Movement;
using GameCore.Mapping.Layers;
using GameCore.Misc;

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

		public override EThinkingResult Thinking()
		{
			AddActToPool(new MoveAct(), new Point(World.Rnd.Next(3) - 1, World.Rnd.Next(3) - 1));
			return EThinkingResult.NORMAL;
		}
	}
}