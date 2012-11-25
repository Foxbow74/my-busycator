using System;

namespace RusLanguage
{
	public static class Variants
	{
		public static string ThereIsWas(ESex _sex, Random _rnd) 
		{
			switch (_sex)
			{
				case ESex.MALE:
					return "там был ";
				case ESex.FEMALE:
					return "там была ";
				case ESex.IT:
					return "там было ";
				default:
					throw new ArgumentOutOfRangeException("_sex");
			}
		}

		public static string HaveGotDamage(INameProvider _nameProvider, int _damage)
		{
			var name = Sklonenia.ToPadej(EPadej.IMEN, _nameProvider.Name, _nameProvider.IsCreature, _nameProvider.Sex);
			switch (_nameProvider.Sex)
			{
				case ESex.MALE:
					return name + " получил " + _damage.Пунктов() + " урона";
					break;
				case ESex.FEMALE:
					return name + " получила " + _damage.Пунктов() + " урона";
					break;
				case ESex.IT:
					return name + " получило " + _damage.Пунктов() + " урона";
					break;
				case ESex.PLURAL:
					return name + " получили " + _damage.Пунктов() + " урона";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static string Died(INameProvider _nameProvider)
		{
			var name = Sklonenia.ToPadej(EPadej.IMEN, _nameProvider.Name, _nameProvider.IsCreature, _nameProvider.Sex);
			switch (_nameProvider.Sex)
			{
				case ESex.MALE:
					return name + " мертв";
					break;
				case ESex.FEMALE:
					return name + " издохла";
					break;
				case ESex.IT:
					return name + " скончалось";
					break;
				case ESex.PLURAL:
					return name + " отдали концы";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public interface INameProvider
	{
		ESex Sex { get; }
		string Name { get; }
		bool IsCreature { get; }
		bool IsUnique { get; }
	}
}
