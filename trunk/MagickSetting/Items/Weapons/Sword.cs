using System;
using GameCore;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Storage;

namespace MagickSetting.Items.Weapons
{
	public class Sword : AbstractMeleeWeapon, ISpecial
	{
		public Sword(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get; protected set;
        }

		public override string Name { get { return "меч"; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,1,4,new Dice(3,4,0));
		}
	}

	public class SwordsProvider : WeaponProvider<Sword>
	{
		protected override Sword CreateT()
		{
			throw new NotImplementedException();
		}

		public override Guid ProvierTypeId
		{
			get { return new Guid("35AD90CD-F4FD-4859-AF13-FECD2C0C858B"); }
		}
	}

	public class AxeProvider : WeaponProvider<Axe>
	{
		protected override Axe CreateT()
		{
			throw new NotImplementedException();
		}

		public override Guid ProvierTypeId
		{
			get { return new Guid("528D866D-5F3D-47DF-9785-0B83CA74AB53"); }
		}
	}
}