using System;
using GameCore;
using GameCore.Creatures;

namespace LanguagePack
{
	public static class Variants
	{
		private static readonly Random m_rnd = new Random();

		public static string ThereIsWas(ESex _sex) 
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

		public static string HaveGotDamage(Creature _nameProvider, int _damage)
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

		public static string Died(Creature _nameProvider)
		{
			var name = Sklonenia.ToPadej(EPadej.IMEN, _nameProvider.Name, _nameProvider.IsCreature, _nameProvider.Sex);
			switch (_nameProvider.Sex)
			{
				case ESex.MALE:
					return name + " мертв";
				case ESex.FEMALE:
					return name + " издохла";
				case ESex.IT:
					return name + " скончалось";
				case ESex.PLURAL:
					return name + " отдали концы";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static string ByLuck()
		{
			switch (m_rnd.Next(4))
			{
				case 0:
					return "чудом";
				case 1:
					return "случайно";
				case 2:
					return "подскользнувшись";
				case 3:
					return "закашлявшись";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
