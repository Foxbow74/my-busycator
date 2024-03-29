﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Creatures.Dummies;
using GameCore.Essences;
using GameCore.Essences.Weapons;
using GameCore.Messages;
using GameCore;

namespace LanguagePack
{
	public static class XMessageCompiler
	{
		static readonly Dictionary<string, Func<XMessage[], string>> m_processors = new Dictionary<string, Func<XMessage[], string>>();
		static readonly Dictionary<string, Func<XMessage[], string>> m_distinctProcessors = new Dictionary<string, Func<XMessage[], string>>();

		static XMessageCompiler()
		{
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), ИзНесколькихАтакНекоторыеОказалисьРезультативны);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURE_TAKES_DAMAGE), ИзНесколькихАтакНекоторыеОказалисьРезультативны);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED), ИзНесколькихАтакНекоторыеОказалисьРезультативныНекоторыеПоглощеныБроней);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO), ВсеАтакиПрошлиНоНеСнялиНиОдногоПункта);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), УвернулсяОтоВсехАтак);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE), ВсеАтакиДостиглиЦели);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED), ВсеАтакиПрошлиНоПоглощеныБроней);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO), ВсеАтакиПрошлиНоПоглощеныБроней);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED), ВсеАтакиПрошлиНоПоглощеныБроней);

			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO), ToDo);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO), ToDo);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURE_TAKES_DAMAGE), ToDo);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO, EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), ToDo);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO, EALTurnMessage.CREATURE_TAKES_DAMAGE), ToDo);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURE_TAKES_DAMAGE,EALTurnMessage.CREATURES_ATTACK_DAMAGE_IS_ZERO), ToDo);
			m_distinctProcessors.Add(PackDistinct(EALTurnMessage.AVATAR_IS_LUCK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED), ToDo);


			m_processors.Add(Pack(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK), АтакаПромах);
			m_processors.Add(Pack(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE), АтакаРанение);
			m_processors.Add(Pack(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURE_KILLED), АтакаСмерть);
			m_processors.Add(Pack(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED), АтакаПоглощенаБроней);

			m_processors.Add(Pack(EALTurnMessage.AVATAR_IS_LUCK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURE_KILLED), КритСмерть);
			m_processors.Add(Pack(EALTurnMessage.AVATAR_IS_LUCK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE), КритРанение);

			m_processors.Add(Pack(EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE), РанениеНеСПервогоРаза);

			m_processors.Add(Pack(EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE, EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED, EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK, EALTurnMessage.CREATURE_TAKES_DAMAGE), ИзНесколькихАтакНекоторыеОказалисьРезультативны);

			m_processors.Add(Pack(EALTurnMessage.CELL_IS_OCCUPIED_BY), КлеткаЗанята);

			m_processors.Add(Pack(EALTurnMessage.CREATURE_TAKES_IT), СуществоВзяло);
			m_processors.Add(Pack(EALTurnMessage.CREATURE_DROPS_IT), СуществоБросило);
			


			
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

		private static string СуществоВзяло(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var item = xMessage.First<Item>();
			var actor = xMessage.Actor;

			if (actor.IsAvatar)
			{
				return EALNouns.YOU.AsNoun() + " " + EALVerbs.TAKES.GetString(EALNouns.YOU.AsNoun(), EVerbType.DONE) + " " + item.GetName(actor).To(EPadej.VIN, true);
			}
			else
			{
				return actor.GetName(World.TheWorld.Avatar).To(EPadej.IMEN) + EALVerbs.TAKES.GetString(actor.Name, EVerbType.DONE) + " " + item.GetName(World.TheWorld.Avatar).To(EPadej.VIN, true);
			}
		}

		private static string СуществоБросило(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var item = xMessage.First<Item>();
			var actor = xMessage.Actor;

			if (actor.IsAvatar)
			{
				return item.GetName(actor).To(EPadej.VIN, true) + " выброшен";
			}
			else
			{
				return actor.GetName(World.TheWorld.Avatar).To(EPadej.IMEN) + EALVerbs.DROPS.GetString(actor.Name, EVerbType.DONE) + " " + item.GetName(World.TheWorld.Avatar).To(EPadej.VIN, true);
			}
		}

		private static string ВсеАтакиДостиглиЦели(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var weapon = (Item)xMessage.First<IWeapon>();
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			var damage = _arg.Where(_a => _a.Type == EALTurnMessage.CREATURE_TAKES_DAMAGE).All<int>().Sum();

			switch (success)
			{
				case 1:
					return string.Format("{0} {1} {4}  {2} на {3}", weapon.To(EPadej.IMEN), actor.To(EPadej.ROD), target.To(EPadej.ROD), damage.Пунктов(), EALVerbs.HURT.GetString(weapon.Name, EVerbType.DONE));
				case 2:
					return string.Format("обе атаки {0} достигли цели, с {1} снято {2}", actor.To(EPadej.ROD), target.To(EPadej.ROD), damage.Пунктов());
				default:
					return string.Format("раз за разом {0} {1} нещадно {5} {2}, сняв за {3} {4}", weapon.To(EPadej.IMEN), actor.To(EPadej.ROD), target.To(EPadej.ROD), success.Атак(), damage.Пунктов(), EALVerbs.HACK.GetString(weapon.Name,EVerbType.IN_PROCESS));
			}
		}

		private static string ВсеАтакиПрошлиНоПоглощеныБроней(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var weapon = (Item)xMessage.First<IWeapon>();
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			switch (success)
			{
				case 1:
					return string.Format("{0} {1} не смог добраться до плоти  {2}", weapon.To(EPadej.IMEN), actor.To(EPadej.ROD), target.To(EPadej.ROD));
				case 2:
					return string.Format("пару раз {0} удавалось пробить защиту {1}, но ни одна из не пробила броню", actor.To(EPadej.DAT), target.To(EPadej.ROD));
				default:
					return string.Format("ни одна из {0} {1} не смогли пробить броню {2}", success.Атак(), actor.To(EPadej.ROD), target.To(EPadej.ROD));
			}
		}

		private static string ВсеАтакиПрошлиНоНеСнялиНиОдногоПункта(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var weapon = (Item)xMessage.First<IWeapon>();
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			switch (success)
			{
				case 1:
					return string.Format("{0} {1} едва не {2} {3}", weapon.To(EPadej.IMEN), actor.To(EPadej.ROD), ERLVerbs.ЗАДЕЛ.GetString(weapon.Name, EVerbType.DONE), target.To(EPadej.ROD));
				case 2:
					return string.Format("пару раз {0} удавалось пробить защиту {1}, но обе атаки не были проведены до конца", actor.To(EPadej.DAT), target.To(EPadej.ROD));
				default:
					return string.Format("атаки {0} слишком слабы, чтобы нанести вред {1}", actor.To(EPadej.ROD), target.To(EPadej.ROD));
			}
		}

		private static string УвернулсяОтоВсехАтак(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var success = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK);
			switch (success)
			{
				case 1:
					return string.Format("Атака {0} успешно парирована {1}", actor.To(EPadej.ROD, true), target.To(EPadej.TVOR, true));
				case 2:
					return string.Format("{0} парировал обе атаки {1}", target.To(EPadej.IMEN), actor.To(EPadej.ROD));
				default:
					return string.Format("{0} провел {1}, но все они были отбиты", actor.To(EPadej.IMEN), success.Атак());
			}
		}

		private static string ИзНесколькихАтакНекоторыеОказалисьРезультативныНекоторыеПоглощеныБроней(XMessage[] _arg)
		{
			var s = ИзНесколькихАтакНекоторыеОказалисьРезультативны(_arg);
			var success = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_DAMAGE_ADSORBED);
			return string.Format("{0} , а {1} не смогли пробить броню", s, success.Атак());
		}

		private static string ИзНесколькихАтакНекоторыеОказалисьРезультативны(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var target = _arg.All<Creature>().Distinct().Single();
			var countOfFails = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_FAILS_DV_TOHIT_CHECK);
			var countOfSuccess = _arg.Count(_a => _a.Type == EALTurnMessage.CREATURES_ATTACK_SUCCESS_DV_TOHIT_CHECK);
			var damage = _arg.Where(_a => _a.Type == EALTurnMessage.CREATURE_TAKES_DAMAGE).All<int>().Sum();

			var l = target.To(EPadej.ROD, true);

			if (countOfSuccess == 1)
			{
				return string.Format("из {0} прошла лишь {1} {2}, с {3} снято {4}", (countOfFails + countOfSuccess), countOfSuccess.Атак(), actor.To(EPadej.ROD), target.To(EPadej.ROD), damage.Пунктов());
			}
			return string.Format("из {0} лишь {1} {2} оказались результативны, с {3} снято {4}", (countOfFails + countOfSuccess), countOfSuccess.Атак(), actor.To(EPadej.ROD), target.To(EPadej.ROD, true), damage.Пунктов());
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
				return string.Format("со второго раза  {0} удается ранить {1} {2} на {3}", actor.To(EPadej.DAT), target.To(EPadej.ROD), weapon.To(EPadej.TVOR), _arg.First<int>().Пунктов());
			}
		}

		private static string КритРанение(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)_arg.First<IWeapon>();
			var target = _arg.First<Creature>();
			if (actor.Is<Missile>())
			{
				return string.Format("{0} {3} в незащищенный участок и снимает {1} {2}", actor.Name.To(EPadej.IMEN), target.To(EPadej.DAT), _arg.First<int>().Пунктов(), EALVerbs.HIT.GetString(actor.Name, EVerbType.IN_PROCESS));
			}
			else
			{
				return string.Format("особо метким ударом {0} {4} {1} {2} на {3}", actor.Name.To(EPadej.IMEN), target.To(EPadej.ROD), weapon.To(EPadej.TVOR), _arg.First<int>().Пунктов(), EALVerbs.HURT.GetString(actor.Name, EVerbType.DONE));
			}
		}

		private static string КритСмерть(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)_arg.First<IWeapon>();
			var target = _arg.First<Creature>();
			if (actor.Is<Missile>())
			{
				return string.Format("{0} на меcте убивает {1}", actor.Name.To(EPadej.IMEN), target.To(EPadej.VIN));
			}
			else
			{
				return string.Format("нанеся чудовищную рану {0} {1} убил {2}", weapon.To(EPadej.TVOR), actor.Name.To(EPadej.IMEN), target.To(EPadej.VIN));
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
			if (actor.Is<Missile>())
			{
				return string.Format("{0} {3} в {1} и {4} на {2}", actor.Name.To(EPadej.IMEN), target.To(EPadej.ROD), _arg.First<int>().Пунктов(), EALVerbs.HIT.GetString(actor.Name, EVerbType.DONE), EALVerbs.HURT.GetString(actor.Name, EVerbType.DONE));
			}
			else
			{
				return string.Format("{0} {4} {1} {2} на {3}", actor.Name.To(EPadej.IMEN), target.To(EPadej.ROD), weapon.To(EPadej.TVOR), _arg.First<int>().Пунктов(), EALVerbs.HURT.GetString(actor.Name, EVerbType.DONE));
			}
		}

		private static string АтакаПромах(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var target = xMessage.First<Creature>();
			if (actor.Is<Missile>())
			{
				return string.Format("{0} пролетел мимо {1}", actor.To(EPadej.IMEN), target.To(EPadej.ROD));
			}
			else
			{
				return string.Format("{0} {2} и {3} по {1}", actor.To(EPadej.IMEN), target.To(EPadej.DAT), EALVerbs.MISS.GetString(actor.Name, EVerbType.DONE), EALVerbs.DONT_HIT.GetString(actor.Name, EVerbType.DONE));
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
				return string.Format("{0} отскакивает от {1}", actor.Name.To(EPadej.IMEN), target.To(EPadej.ROD));
			}
			else
			{
				return string.Format("{0} {3} {1} по {2}, но не пробил броню", actor.Name.To(EPadej.IMEN), weapon.To(EPadej.TVOR), target.To(EPadej.DAT), EALVerbs.STIKE.GetString(actor.Name, EVerbType.DONE));
			}
		}

		private static string PackDistinct(params EALTurnMessage[] _types)
		{
			return _types.Distinct().OrderBy(_e => _e).Select(_t => new string((char)('A' + (char)(int)_t), 1)).Aggregate((_s, _s1) => _s + _s1);
		}

		private static string Pack(params EALTurnMessage[] _types)
		{
			return _types.Pack();
		}

		private static string Pack(this IEnumerable<EALTurnMessage> _types)
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

		private static string КлеткаЗанята(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var essence = xMessage.FirstOrDefault<Essence>();
			if (essence != null)
			{
				if (essence is Creature)
				{
					return "из-за " + essence.To(EPadej.ROD) + " сюда никак не пройти";
				}
				return "Вы уперлись в " + essence.To(EPadej.VIN);
			}
			return "Не пройти, здесь " + ((Noun)_arg[0].Params[0]).To(EPadej.IMEN);
		}


		private static string АтакаСмерть(XMessage[] _arg)
		{
			var xMessage = _arg[0];
			var actor = xMessage.Actor;
			var weapon = (Item)xMessage.First<IWeapon>();
			var target = xMessage.First<Creature>();
			if (actor.Is<AbstractDummyCreature>())
			{
				return string.Format("{0} {2} {1}", actor.Name.To(EPadej.IMEN), target.To(EPadej.VIN), EALVerbs.FINISH.GetString(actor.Name, EVerbType.DONE));
			}
			else
			{
				return string.Format("нанеся {0} смертельную рану {1} убил {2}", weapon.To(EPadej.TVOR), actor.Name.To(EPadej.IMEN), target.To(EPadej.VIN));
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
