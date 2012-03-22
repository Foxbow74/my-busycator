using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Mapping.Layers;
using GameCore.Misc;
using GameCore.Objects;
using RusLanguage;

namespace GameCore.Creatures
{
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

		protected Intelligent(WorldLayer _layer, int _speed, EIntellectGrades _intellectGrades)
			: base(_layer, _speed)
		{
			Sex = World.Rnd.Next(2)==0?ESex.MALE : ESex.FEMALE;
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

		public Item this[EEquipmentPlaces _places]
		{
			get { return m_equipment[_places]; }
		}

		/// <summary>
		/// Добавить в рюкзак и затем экипироваться
		/// </summary>
		/// <param name = "_place"></param>
		/// <param name = "_item"></param>
		protected void Equip(EEquipmentPlaces _place, Item _item)
		{
			if (_item is FakedItem)
			{
				_item = (Item)((FakedItem) _item).ResolveFake(this);
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

		public override IEnumerable<ThingDescriptor> GetBackPackItems()
		{
			return m_backPack.GetItems(this).Items.Select(_item => new ThingDescriptor(_item, LiveCoords, m_backPack));
		}

		public IEnumerable<Tuple<EEquipmentPlaces, Item>> GetEquipment()
		{
			return m_equipment.Select(_item => new Tuple<EEquipmentPlaces, Item>(_item.Key, _item.Value));
		}

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
				throw new ApplicationException("Нельзя экипировать '" + _item.GetName(this) + "' как '" + equipmentPlacesAttribute.DisplayName + "'");
			}

			var item = m_equipment[_place];
			if (item != null)
			{
				throw new ApplicationException("Одно поверх другого?");
			}
			m_backPack.GetItems(this).Remove(_item);
			m_equipment[_place] = _item;
		}

		public void RemoveFromBackpack(Item _item)
		{
			m_backPack.GetItems(this).Remove(_item);
		}

		public override ILightSource Light
		{
			get
			{
				var tool = this[EEquipmentPlaces.TOOL];
				if (tool != null && tool.Light!=null)
				{
					return tool.Light;
				}
				return base.Light;
			}
		}

		public override string Name
		{
			get
			{
				return IntelligentName +", " + Roles.First().Name;
			}
		}

		public abstract string IntelligentName { get; }
	}
}