using GameCore.AbstractLanguage;

namespace GameCore.Essences.Weapons
{
	public interface IWeapon
	{
		/// <summary>
		/// ������, ��������������� ����, ��������� �������
		/// </summary>
		EALVerbs Verb { get; }
	}
}