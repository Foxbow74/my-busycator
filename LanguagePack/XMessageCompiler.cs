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
		static readonly Dictionary<string, Func<XMessage[], string>> m_distinctProcessors = new Dictionary<string, Func<XMessage[], string>>();

		static XMessageCompiler()
		{
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), ИзНесколькихАтакНекоторыеОказалисьРезультативны);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURE_TAKES_DAMAGE), ИзНесколькихАтакНекоторыеОказалисьРезультативны);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED), ИзНесколькихАтакНекоторыеОказалисьРезультативныНекоторыеПоглощеныБроней);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO), ВсеАтакиПрошлиНоНеСнялиНиОдногоПункта);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), УвернулсяОтоВсехАтак);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), ВсеАтакиДостиглиЦели);

			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED), ВсеАтакиПрошлиНоПоглощеныБроней);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO), ВсеАтакиПрошлиНоПоглощеныБроней);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED), ВсеАтакиПрошлиНоПоглощеныБроней);

			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO), ToDo);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO), ToDo);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURE_TAKES_DAMAGE), ToDo);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), ToDo);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO, EXMType.CREATURE_TAKES_DAMAGE), ToDo);
			m_distinctProcessors.Add(PackDistinct(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURE_TAKES_DAMAGE,EXMType.CREATURES_ATTACK_DAMAGE_IS_ZERO), ToDo);

			


			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), АтакаПромах);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), АтакаРанение);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURE_KILLED), АтакаСмерть);
			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED), АтакаПоглощенаБроней);

			m_processors.Add(Pack(EXMType.AVATAR_IS_LUCK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURE_KILLED), КритСмерть);
			m_processors.Add(Pack(EXMType.AVATAR_IS_LUCK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), КритРанение);

			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), РанениеНеСПервогоРаза);

			m_processors.Add(Pack(EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE, EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURE_TAKES_DAMAGE), ИзНесколькихАтакНекоторыеОказалисьРезультативны);


			


			
			//EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED


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

		private static string ToDo(XMessage[] _arg)
		{
			return "TODO:";
		}

		private static string ВсеАтакиДостиглиЦели(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var weapon = (Item)xMessage.First<IWeapon>();
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			var damage = _arg.Where(_a => _a.Type == EXMType.CREATURE_TAKES_DAMAGE).All<int>().Sum();

			switch (success)
			{
				case 1:
					return weapon.To(EPadej.IMEN) + " " + actor.To(EPadej.ROD) + " ранили  " + target.To(EPadej.ROD) + " на " + damage.Пунктов();
				case 2:
					return "оба удара " + actor.To(EPadej.ROD) + " достигли цели, с " + target.To(EPadej.ROD) + " снято " + damage.Пунктов();
				default:
					return "раз за разом "  + weapon.To(EPadej.IMEN) + " " + actor.To(EPadej.ROD) + " нещадно кромсали " + target.To(EPadej.ROD) + ", сняв за " + success.Атак() + " " + damage.Пунктов();
			}
		}

		private static string ВсеАтакиПрошлиНоПоглощеныБроней(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var weapon = (Item)xMessage.First<IWeapon>();
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			switch (success)
			{
				case 1:
					return weapon.To(EPadej.IMEN) + " " + actor.To(EPadej.ROD) + " не смог добраться до плоти  " + target.To(EPadej.ROD);
				case 2:
					return "пару раз " + actor.To(EPadej.DAT) + " удавалось пробить защиту " + target.To(EPadej.ROD) + ", но ни одна из не пробили броню";
				default:
					return "ни одна из " + success.Атак() + " " + actor.To(EPadej.ROD) + " не смогли пробить броню " + target.To(EPadej.ROD);
			}
		}

		private static string ВсеАтакиПрошлиНоНеСнялиНиОдногоПункта(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var weapon = (Item)xMessage.First<IWeapon>();
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			switch (success)
			{
				case 1:
					return weapon.To(EPadej.IMEN) + " " + actor.To(EPadej.ROD) + " едва не задел  " + target.To(EPadej.ROD);
				case 2:
					return "пару раз " + actor.To(EPadej.DAT) + " удавалось пробить защиту " + target.To(EPadej.ROD) + ", но обе атаки не были проведены до конца";
				default:
					return "атаки " + actor.To(EPadej.ROD) + " слишком слабы, чтобы нанести вред " + target.To(EPadej.ROD);
			}
		}

		private static string УвернулсяОтоВсехАтак(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK);
			switch (success)
			{
				case 1:
					return "Атака " + actor.To(EPadej.ROD) + " успешно парирована " + target.To(EPadej.TVOR);
				case 2:
					return target.To(EPadej.IMEN) + " парировал обе атаки " + actor.To(EPadej.ROD);
				default:
					return actor.To(EPadej.IMEN) + " провел " + success + " атак, но все они были отбиты";
			}
		}

		private static string ИзНесколькихАтакНекоторыеОказалисьРезультативныНекоторыеПоглощеныБроней(XMessage[] _arg)
		{
			var s = ИзНесколькихАтакНекоторыеОказалисьРезультативны(_arg);
			var success = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_DAMAGE_ADSORBED);
			return s + " , а " + success.Атак() + " не смогли пробить броню";
		}

		private static string ИзНесколькихАтакНекоторыеОказалисьРезультативны(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var countOfFails = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK);
			var countOfSuccess = _arg.Count(_a => _a.Type == EXMType.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			var damage = _arg.Where(_a => _a.Type == EXMType.CREATURE_TAKES_DAMAGE).All<int>().Sum();

			return "из " + (countOfFails + countOfSuccess) + " лишь " + countOfSuccess.Атак() + "  " + actor.To(EPadej.ROD) + " оказались результативны, с " + target.To(EPadej.ROD) + " снято " + damage.Пунктов();
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
				return "нанеся чудовищную рану " + weapon.To(EPadej.TVOR) + " " + actor.Name + " убил " + target.To(EPadej.VIN);
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

		private static string PackDistinct(params EXMType[] _types)
		{
			return _types.Distinct().OrderBy(_e => _e).Select(_t => new string((char)('A' + (char)(int)_t), 1)).Aggregate((_s, _s1) => _s + _s1);
		}

		private static string Pack(params EXMType[] _types)
		{
			return _types.Pack();
		}

		private static string Pack(this IEnumerable<EXMType> _types)
		{
			var array = _types.ToArray();
			var s = array.Select(_t => new string((char) ('A' + (char) (int) _t), 1)).Aggregate((_s, _s1) => _s + _s1);

#if DEBUG
			//var dist = PackDistinct(array);
			//if (dist.Length < array.Length)
			//{
			//    if (m_distinctProcessors.ContainsKey(dist))
			//    {
			//        throw new ApplicationException("Такой обработчик уже есть :" + "EXMType." + string.Join(", EXMType.", array.Select(_t => _t.ToString())));
			//    }
			//}
#endif
			return s;
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

				var dkey = PackDistinct(chain);
				if (m_distinctProcessors.TryGetValue(dkey, out func))
				{
					yield return func(@group.ToArray());
					continue;
				}
				
				Debug.WriteLine("EXMType." + string.Join(", EXMType.", chain.Select(_t => _t.ToString())));
				m_processors.Add(key, _messages => "???");

				sb.Clear();
				sb.Append(group.Key);
				foreach (var message in group)
				{
					sb.AppendFormat(" {0}[{1}]", message.Type, string.Join(", ", message.Params.Select(_o => _o.ToString())));
				}
				yield return sb.ToString();
			}
		}
	}
}
