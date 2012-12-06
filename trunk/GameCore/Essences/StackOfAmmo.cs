using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences.Weapons;

namespace GameCore.Essences
{
	public abstract class StackOfAmmo : StackOfItems, IWeapon
	{
		protected StackOfAmmo(EALNouns _nameOfItem, Material _material) : base(_nameOfItem, _material) { }

        public override ETileset Tileset
        {
            get { return ETileset.MISSILES; }
        }

		public override EItemCategory Category { get { return EItemCategory.MISSILES; } }

		protected override int GetStartCount(Creature _creature) { return (int) (_creature.GetLuckRandom*25) + 1; }

		public virtual EALVerbs Verb
		{
			get { return EALVerbs.AMMO_WEAPON_VERB; }
		}
	}
}