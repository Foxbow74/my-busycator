using System;
using GameCore;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Storage.XResourceEssences;

namespace MagickSetting.Items.Weapons
{
	public class Sword : AbstractMeleeWeapon, ISpecial
	{
		private readonly string m_name;

		public Sword(Material _material, string _name, int _tileIndex)
			: base(_material)
		{
			TileIndex = _tileIndex;
			m_name = _name;
		}

		public override int TileIndex
        {
            get; protected set;
        }

		public override string Name { get { return m_name; } }

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return new ItemBattleInfo(0,0,1,4,new Dice(3,4,0));
		}

		public override FColor LerpColor
		{
			get
			{
				return Material==null?FColor.Red:base.LerpColor;
			}
			protected set
			{
				base.LerpColor = value;
			}
		}
	}

	public class XResourceSword : XResourceMeleeWeapon<Sword>
	{
		protected override Sword CreateT(Material _material)
		{
			return new Sword(_material, Name, TileIndex);
		}

		public override Guid ProvierTypeId
		{
			get { return new Guid("35AD90CD-F4FD-4859-AF13-FECD2C0C858B"); }
		}
	}

	public class XResourceAxe : XResourceMeleeWeapon<Axe>
	{
		protected override Axe CreateT(Material _material)
		{
			throw new NotImplementedException();
		}

		public override Guid ProvierTypeId
		{
			get { return new Guid("528D866D-5F3D-47DF-9785-0B83CA74AB53"); }
		}
	}
}