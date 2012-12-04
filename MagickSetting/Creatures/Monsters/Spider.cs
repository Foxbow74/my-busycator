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
	class Spider : AbstractMonster
	{
		private static readonly IWeapon m_jaws = new Jaws(new ItemBattleInfo(0, 0, 0, 5, new Dice(2, 3, 0)));

		public Spider(WorldLayer _layer)
			: base("паук".AsNoun(ESex.MALE, true), _layer, 120 + World.Rnd.Next(20))
		{
			var lcd = 0.5f + (Nn % 20 - 10) / 10f;
			LerpColor = new FColor(0.3f, lcd * (float)World.Rnd.NextDouble()/2, lcd * (float)World.Rnd.NextDouble(), lcd * (float)World.Rnd.NextDouble()/2);

			Behaviour = EMonsterBehaviour.IDLE;
		}


		public override int TileIndex
		{
			get
			{
				return 2;
			}
		}

		public override CreatureBattleInfo CreateBattleInfo()
		{
			return new CreatureBattleInfo(this, 4, 2, new Dice(1,8));
		}

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			yield return m_jaws;
		}
	}
}