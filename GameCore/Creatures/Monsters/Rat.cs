using System.Collections.Generic;
using GameCore.Battle;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;

namespace GameCore.Creatures.Monsters
{
	class Rat : AbstractMonster
	{
		static IWeapon m_jaws = new Jaws(new ItemBattleInfo(0, 0, 0, 5, new Dice(2, 3, 0)));

		public Rat(WorldLayer _layer)
			: base(_layer, 120 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 3 - 1) / 10f;
			LerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble());

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

		internal override CreatureBattleInfo CreateBattleInfo()
		{
			return new CreatureBattleInfo(this, 4, 2, new Dice(1,2));
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			throw new System.NotImplementedException();
		}
	}
}