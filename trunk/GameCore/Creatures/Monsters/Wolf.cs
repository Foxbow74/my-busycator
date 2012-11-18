using GameCore.Mapping.Layers;

namespace GameCore.Creatures.Monsters
{
	class Wolf:Monster
	{
		public Wolf(WorldLayer _layer)
			: base(_layer, 80 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 10 - 5) / 10f;
			m_lerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble() / 2f);

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
	}

	class Rat : Monster
	{
		public Rat(WorldLayer _layer)
			: base(_layer, 120 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 3 - 1) / 10f;
			m_lerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble());

			Behaviour = EMonsterBehaviour.ATACK_AVATAR;
		}


		public override int TileIndex
		{
			get
			{
				return 1;
			}
		}

		public override string Name
		{
			get
			{
				return "крыса";
			}
		}
	}

	class Spider : Monster
	{
		public Spider(WorldLayer _layer)
			: base(_layer, 120 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 20 - 10) / 10f;
			m_lerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble()/2, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble()/2);

			Behaviour = EMonsterBehaviour.ATACK_AVATAR;
		}


		public override int TileIndex
		{
			get
			{
				return 2;
			}
		}

		public override string Name
		{
			get
			{
				return "паук";
			}
		}
	}
}
