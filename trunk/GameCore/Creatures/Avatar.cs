using System;
using GameCore.Battle;
using GameCore.CreatureRoles;
using GameCore.Essences;
using GameCore.Essences.Ammo;
using GameCore.Essences.Tools;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;

namespace GameCore.Creatures
{
	public class Avatar : Intelligent, ISpecial
	{
		public Avatar(WorldLayer _surface)
			: base(_surface, 100, EIntellectGrades.INT)
		{
			Tactic = ETactics.NORMAL;
			Luck = 6;
			AddRole(new AvatarRole());
			Equip(EEquipmentPlaces.RIGHT_HAND, EssenceHelper.GetRandomFakedItem<AbstractMeleeWeapon>(World.Rnd));
			Equip(EEquipmentPlaces.MISSILE_WEAPON, EssenceHelper.GetFirstFoundedItem<AbstractRangedWeapon>());

			var ammo = EssenceHelper.GetRandomFakedItem<StackOfAmmo>(World.Rnd).ResolveFake(this) as StackOfAmmo;
			ammo.Count = 100;
			Equip(EEquipmentPlaces.MISSILES, ammo);
			Equip(EEquipmentPlaces.TOOL, EssenceHelper.GetFirstFoundedItem<AbstractTorch>());
		}

		public override ETileset Tileset { get { return ETileset.AVATAR; } }

        public override int TileIndex { get { return 2; } }

		public override string IntelligentName { get { return "Герой"; } }

		public override EFraction Fraction
		{
			get { return EFraction.AVATAR; }
		}

		public ETactics Tactic { get; set; }

		public int LuckModifier
		{
			get { return Luck/3; }
		}

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }

		public override EThinkingResult Thinking() { throw new NotImplementedException(); }

		public override CreatureBattleInfo CreateBattleInfo()
		{
			return new IntelligentBattleInfo(this,3,1,new Dice(5,8,20));
		}

	}
}