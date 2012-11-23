using GameCore.Battle;
using GameCore.Mapping.Layers;

namespace GameCore.Creatures.Monsters
{
	class Wolf:AbstractMonster
	{
		public Wolf(WorldLayer _layer)
			: base(_layer, 80 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 10 - 5) / 10f;
			LerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble() / 2f);

			Behaviour = EMonsterBehaviour.ATACK_AVATAR;
		}


		public override int TileIndex
		{
			get
			{
				return 0;
			}
		}

		public override string Name
		{
			get
			{
				return "волк";
			}
		}

		internal override CreatureBattleInfo CreateBattleInfo()
		{
			return new CreatureBattleInfo(this, 4, 2, 5, new Dice(2, 3, 0), new Dice(2,8,4));
		}
	}
}
