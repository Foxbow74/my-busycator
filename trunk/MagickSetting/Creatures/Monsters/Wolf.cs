using System.Collections.Generic;
using GameCore;
using GameCore.AbstractLanguage;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;
using MagickSetting.Items.Weapons.NaturalWeapon;

namespace MagickSetting.Creatures.Monsters
{
	class Wolf:AbstractMonster
	{
		private static readonly IWeapon m_jaws = new Jaws(new ItemBattleInfo(0, 0, 0, 5, new Dice(2, 3, 0)));

		public Wolf(WorldLayer _layer)
			: base(EALNouns.Wolf, _layer, 80 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 10 - 5) / 10f;
			LerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble() / 2f);

			Behaviour = EMonsterBehaviour.IDLE;
		}


		public override int TileIndex
		{
			get
			{
				return 0;
			}
		}

		public override CreatureBattleInfo CreateBattleInfo()
		{
			return new CreatureBattleInfo(this, 4, 2, new Dice(2,8,4));
		}
		
		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			yield return m_jaws;
		}

	}
}
