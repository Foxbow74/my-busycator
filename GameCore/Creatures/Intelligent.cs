﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Essences;
using GameCore.Essences.Faked;
using GameCore.Essences.Weapons;
using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Creatures
{
    /// <summary>
    /// Существо, достаточно интеллектуальное, чтобы иметь инвентарь и использовать предметы
    /// </summary>
	public abstract class Intelligent : Creature
	{
    	#region EIntellectGrades enum

    	public enum EIntellectGrades
		{
			//Может использовать орудия труда и оружие в лапах
			SEMI_INT,
			//Может использовать рюкзак
			INT,
		}

    	#endregion

    	private readonly BackPack m_backPack;

		private readonly Dictionary<EEquipmentPlaces, Item> m_equipment = new Dictionary<EEquipmentPlaces, Item>();
		private readonly EIntellectGrades m_intellectGrades;

		protected Intelligent(EALNouns _name, WorldLayer _layer, int _speed, EIntellectGrades _intellectGrades)
			: base(_name, _layer, _speed)
		{
			Sex = World.Rnd.Next(2) == 0 ? ESex.MALE : ESex.FEMALE;
			m_intellectGrades = _intellectGrades;
			switch (_intellectGrades)
			{
				case EIntellectGrades.SEMI_INT:
					m_equipment.Add(EEquipmentPlaces.RIGHT_HAND, null);
					m_equipment.Add(EEquipmentPlaces.LEFT_HAND, null);
					break;
				case EIntellectGrades.INT:
					m_backPack = new BackPack();

					foreach (var eEquipmentPlacese in EquipmentPlacesAttribute.AllValues)
					{
						m_equipment.Add(eEquipmentPlacese, null);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("_intellectGrades");
			}
		}

		public Item this[EEquipmentPlaces _places] { get { return m_equipment[_places]; } }

		public override ILightSource Light
		{
			get
			{
				var tool = this[EEquipmentPlaces.TOOL];
				if (tool != null && tool.Light != null)
				{
					return tool.Light;
				}
				return base.Light;
			}
		}

		/// <summary>
		/// 	Добавить в рюкзак и затем экипироваться
		/// </summary>
		/// <param name = "_place"></param>
		/// <param name = "_item"></param>
		protected void Equip(EEquipmentPlaces _place, Item _item)
		{
			if (_item is FakedItem)
			{
				_item = (Item)((FakedItem)_item).Essence.Clone(this);
			}
			//_item.Resolve(this);
			ObjectTaken(_item);
			TakeOn(_place, _item);
		}

		public void ObjectTaken(Item _item)
		{
			if (m_intellectGrades >= EIntellectGrades.INT)
			{
				m_backPack.GetItems(this).Add(_item);
			}
			else
			{
				throw new ApplicationException("Тупой");
			}
		}

		public override IEnumerable<EssenceDescriptor> GetBackPackItems() { return m_backPack.GetItems(this).Items.Select(_item => new EssenceDescriptor(_item, GeoInfo[0,0], m_backPack, this)); }

		public IEnumerable<Tuple<EEquipmentPlaces, Item>> GetEquipment() { return m_equipment.Select(_item => new Tuple<EEquipmentPlaces, Item>(_item.Key, _item.Value)); }

		public void TakeOff(EEquipmentPlaces _place)
		{
			var item = m_equipment[_place];
			if (item == null)
			{
				throw new ApplicationException("Чего снять?");
			}
			m_equipment[_place] = null;
			m_backPack.GetItems(this).Add(item);
		}

		public void TakeOn(EEquipmentPlaces _place, Item _item)
		{
			var equipmentPlacesAttribute = EquipmentPlacesAttribute.GetAttribute(_place);
			if (!equipmentPlacesAttribute.IsAbleToEquip(_item.Category))
			{
				throw new ApplicationException("Нельзя экипировать '" + _item.GetName(this) + "' как '" + EALSentence.NONE.GetString(_place.AsNoun()) + "'");
			}

			var item = m_equipment[_place];
			if (item != null)
			{
				throw new ApplicationException("Одно поверх другого?");
			}
			m_backPack.GetItems(this).Remove(_item);
			m_equipment[_place] = _item;
		}

		public void RemoveFromBackpack(Item _item) { m_backPack.GetItems(this).Remove(_item); }

		public override IEnumerable<IWeapon> GetWeapons(Creature _against)
		{
			Item item;
			if (m_equipment.TryGetValue(EEquipmentPlaces.RIGHT_HAND, out item) && item is IWeapon)
			{
				yield return (IWeapon)item;
			}
			if (m_equipment.TryGetValue(EEquipmentPlaces.LEFT_HAND, out item) && item is IWeapon)
			{
				yield return (IWeapon)item;
			}
		}
	}
}