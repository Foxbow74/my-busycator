using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Messages;
using GameCore.XLanguage;

namespace LanguagePack
{
	public static class XMessageCompiler
	{
		static readonly Dictionary<string, Func<XMessage[], string>> m_processors = new Dictionary<string, Func<XMessage[], string>>();

		static XMessageCompiler()
		{
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), АтакаПромах);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), АтакаРанение);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURE_KILLED), АтакаСмерть);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED), АтакаПоглощенаБроней);

			m_processors.Add(Pack(EXMType.AVATAR_IS_LUCK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURE_KILLED), КритСмерть);
			m_processors.Add(Pack(EXMType.AVATAR_IS_LUCK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), КритРанение);

			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), РанениеНеСПервогоРаза);

			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), ИзНесколькихАтакНекоторыеОказалисьРезультативны);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), ИзНесколькихАтакНекоторыеОказалисьРезультативны);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), ИзНесколькихАтакНекоторыеОказалисьРезультативны);

			//EXMType.AVATAR_IS_LUCK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO
			//EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE
		}

		private static string ИзНесколькихАтакНекоторыеОказалисьРезультативны(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var countOfFails = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK);
			var countOfFailSuccess = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);

			return "из " + (countOfFails + countOfFailSuccess) + " лишь " + countOfFailSuccess + " атаки  " + actor.To(EPadej.ROD) + " оказались результативны, с " + target.To(EPadej.ROD) + " снято " + _arg.All<int>().Sum().Пунктов();
		}

		private static string РанениеНеСПервогоРаза(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)xMessage.First<IWeapon>();
			var target = xMessage.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return "со второго раза  " + actor.To(EPadej.DAT) + " удается ранить " + target.To(EPadej.ROD) + " " + weapon.To(EPadej.TVOR) + " на " + _arg.First<int>().Пунктов();
			}
		}

		private static string КритРанение(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)_arg.First<IWeapon>();
			var target = _arg.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return "особо метким ударом " + actor.Name + " ранил " + target.To(EPadej.ROD) + " " + weapon.To(EPadej.TVOR) + " на " + _arg.First<int>().Пунктов();
			}
		}

		private static string КритСмерть(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)_arg.First<IWeapon>();
			var target = _arg.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return "нанеся чудовищную рану " + weapon.To(EPadej.TVOR) + " " + actor.Name + " убил " + target.To(EPadej.ROD);
			}
		}

		private static T First<T>(this IEnumerable<XMessage> _args)
		{
			return _args.SelectMany(_arg => _arg.Params).OfType<T>().First();
		}

		private static IEnumerable<T> All<T>(this IEnumerable<XMessage> _args)
		{
			return _args.SelectMany(_arg => _arg.Params).OfType<T>();
		}

		private static string АтакаРанение(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)xMessage.First<IWeapon>();
			var target = xMessage.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return actor.Name + " ранил " + target.To(EPadej.ROD) + " " + weapon.To(EPadej.TVOR) + " на " + _arg.First<int>().Пунктов();
			}
		}

		private static string АтакаПромах(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)xMessage.First<IWeapon>();
			var target = xMessage.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return actor.To(EPadej.IMEN) + " промахнулся и не попал по " + target.To(EPadej.DAT);
			}
		}

		private static string АтакаПоглощенаБроней(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)xMessage.First<IWeapon>();
			var target = xMessage.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return actor.Name + " ударил " + weapon.To(EPadej.TVOR) + " по " + target.To(EPadej.DAT) + ", но не пробил броню";
			}
		}

		private static string Pack(params EXMType[] _types)
		{
			return _types.Pack();
		}

		private static string Pack(this IEnumerable<EXMType> _types)
		{
			return _types.Select(_t => new string((char) ('A' + (char) (int) _t), 1)).Aggregate((_s, _s1) => _s + _s1);
		}

		private static string АтакаСмерть(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)xMessage.First<IWeapon>();
			var target = xMessage.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				throw new NotImplementedException();
			}
			else
			{
				return "нанеся смертельную рану " + weapon.To(EPadej.TVOR)  + " " + actor.Name + " убил " + target.To(EPadej.ROD);
			}
		}

		public static IEnumerable<string> Compile(List<XLangMessage> _xlist)
		{
			var messages = _xlist.Select(_xlm => _xlm.XMessage);

			var byActor = messages.GroupBy(_m => _m.Actor);

			var sb = new StringBuilder();

			foreach (var group in byActor)
			{
				var chain = group.Select(_m => _m.Type).ToArray();
				var key = chain.Pack();
				Func<XMessage[], string> func;
				if (m_processors.TryGetValue(key, out func))
				{
					yield return func(group.ToArray());
					continue;
				}
				else
				{
					Debug.WriteLine("EXMType." + string.Join(", EXMType.", chain.Select(_t => _t.ToString())));
					m_processors.Add(key, _messages => "???");
				}

				sb.Append(group.Key);
				foreach (var message in group)
				{
					sb.AppendFormat(" {0}[{1}]", message.Type, string.Join(", ", message.Params.Select(_o => _o.ToString())));
				}
				yield return sb.ToString();
				sb.Clear();
			}
		}
	}
}
