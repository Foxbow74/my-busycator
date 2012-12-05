using GameCore.AbstractLanguage;

namespace GameCore.Essences.Weapons
{
	public abstract class AbstractMeleeWeapon : AbstractWeapon
	{
		protected AbstractMeleeWeapon(EALNouns _name, Material _material) : base(_name, _material) { }

		public override ETileset Tileset
		{
			get
			{
				return ETileset.WEAPONS;
			}
		}
	}
}