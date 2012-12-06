using GameCore.AbstractLanguage;

namespace GameCore.Essences.Weapons
{
	public interface IWeapon
	{
		/// <summary>
		/// √лагол, характеризующий удар, наносимый оружием
		/// </summary>
		EALVerbs Verb { get; }
	}
}