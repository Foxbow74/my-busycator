using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences.Faked;
using GameCore.Essences.Things;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Essences
{
	public static class EssenceHelper
	{
		#region Fields

		private static readonly List<FakedCreature> m_fakedCreatures = new List<FakedCreature>();
		private static readonly List<FakedItem> m_fakedItems = new List<FakedItem>();
		private static readonly List<FakedThing> m_fakedThings = new List<FakedThing>();
		private static readonly List<Material> m_materials = new List<Material>();

		#endregion

		#region .ctor

		static EssenceHelper()
		{
			foreach (var type in Util.GetAllTypesOf<Material>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;

				var material = (Material) Activator.CreateInstance(type);
				m_materials.Add(material);
			}

			foreach (var type in Util.GetAllTypesOf<Essence>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;
				RegisterEssenceType(type);
			}

			if (Constants.GAME_MODE)
			{
				foreach (var essenceProviderHelper in World.XResourceRoot.EssenceProviders)
				{
					var essenceProvider = essenceProviderHelper.GetResourceEssence();
					if (essenceProvider.MaterialTypes == EMaterialType.UNIQ)
					{
						Add(essenceProvider.Create(null));
					}
					else
					{
						foreach (
							var material in
								GetAllowedMaterialTypes(essenceProvider.MaterialTypes)
									.SelectMany(_material => m_materials.Where(_m => _m.MaterialType == _material)))
						{
							Add(essenceProvider.Create(material));
						}
					}
				}
			}
		}

		private static void Add(Essence _essence)
		{
			if (_essence is Creature)
			{
				m_fakedCreatures.Add(new FakedCreature(_essence));
			}
			else 
			if (_essence is Item)
			{
			    m_fakedItems.Add(new FakedItem(_essence));
			}
			else 
			{
				m_fakedThings.Add(new FakedThing(_essence));
			}
		}

		#endregion

		#region Methods

		public static bool CanBeClosed(this Essence _essence, LiveMapCell _cell, Creature _creature)
		{
			if (!(_essence is Thing)) return false;
			if (_essence.IsFake())
			{
				_essence = _cell.ResolveFakeThing(_creature);
			}
			return _essence is ICanbeClosed && ((ICanbeClosed) _essence).ELockType == ELockType.OPEN;
		}

		private static IEnumerable<EMaterialType> GetAllowedMaterialTypes(EMaterialType _materialsType)
		{
			var allowedMaterials = (from EMaterialType value in Enum.GetValues(typeof (EMaterialType)) where _materialsType.HasFlag(value) select value).ToArray();
			return allowedMaterials;
		}

		public static FakedItem GetRandomFakedItem(Random _rnd)
		{
			return m_fakedItems.ToArray().RandomItem(_rnd);
		}

		public static FakedItem GetRandomFakedItem<T>(Random _rnd) where T : Item
		{
			return m_fakedItems.Where(_item=>_item.Is<T>()).ToArray().RandomItem(_rnd);
		}

		public static FakedCreature GetRandomFakedCreature<T>(Random _rnd) where T:Creature
		{
			return m_fakedCreatures.Where(_creature => _creature.Is<T>()).ToArray().RandomItem(_rnd);
		}

		public static Essence GetFakedThing(Random _rnd)
		{
			return m_fakedThings.ToArray().RandomItem(_rnd);
		}

		public static FakedThing GetFirstFoundedThing<T>() where T : Thing
		{
			return m_fakedThings.First(_thing => _thing.Is<T>());
		}

		public static FakedItem GetFirstFoundedItem<T>() where T : Item
		{
			return m_fakedItems.First(_item => _item.Is<T>());
		}
		
		public static FakedCreature GetFirstFoundedCreature<T>() where T : Creature
		{
			return m_fakedCreatures.First(_item => _item.Is<T>());
		}

		public static Material GetFirstFoundedMaterial<T>() where T : Material
		{
			return m_materials.First(_item => _item is T);
		}

		public static IEnumerable<FakedThing> GetAllThings<T>() where T : Thing
		{
			return m_fakedThings.Where(_thing => _thing.Is<T>());
		}

		public static IEnumerable<FakedItem> GetAllItems<T>() where T : Item
		{
			return m_fakedItems.Where(_item => _item.Is<T>());
		}

		public static IEnumerable<FakedCreature> GetAllCreatures<T>() where T : Creature
		{
			return m_fakedCreatures.Where(_item => _item.Is<T>());
		}

		public static TMaterial GetMaterial<TMaterial>() where TMaterial : Material
		{
			return (TMaterial) m_materials.First(_material => _material is TMaterial);
		}

		/// <summary>
		/// Возвращает имя объекта
		/// </summary>
		/// <param name="_essence"> </param>
		/// <param name="_creature"> </param>
		/// <returns> </returns>
		public static Noun GetName(this Essence _essence, Creature _creature)
		{
			if (_essence is IFaked)
			{
				_essence = ((IFaked)_essence).Essence;
			}
			if(_essence is Creature)
			{
				if(_essence is Intelligent)
				{
					return _essence.Name;
				}
				return _essence.Name;
			}
			if(_essence.Name==null)
			{
				
			}
			return _essence.Name;
		}

		public static bool Is<T>(this Essence _essence)
		{
			return _essence != null && _essence.Is<T>();
		}

		public static bool IsLockedFor(this Essence _essence, LiveMapCell _cell, Creature _creature)
		{
			if (_essence == null) return false;
			if (_essence.IsFake())
			{
				_essence = _cell.ResolveFakeThing(_creature);
			}
			return _essence is ICanbeOpened && ((ICanbeOpened) _essence).ELockType != ELockType.OPEN;
		}

		public static bool IsFake(this Essence _essence)
		{
			return _essence is IFaked;
		}

		private static void RegisterEssenceType(Type _type)
		{
			var athing = (Essence)Activator.CreateInstance(_type, new object[] { null });
			if (typeof(Creature).IsAssignableFrom(_type))
			{
				Add(athing);
			}
			else
			{
				foreach (var mtp in GetAllowedMaterialTypes(athing.AllowedMaterialsType))
				{
					var mtpLocal = mtp;
					foreach (var material in m_materials)
					{
						if (material.MaterialType != mtpLocal)
						{
							continue;
						}
						var thing = (Essence) Activator.CreateInstance(_type, material);
						Add(thing);
					}
				}
			}
		}

		#endregion

		#region Properties

		public static IEnumerable<Essence> AllEssences
		{
			get
			{
				foreach (var type in Util.GetAllTypesOf<Essence>())
				{
					if (typeof (ISpecial).IsAssignableFrom(type)) continue;

					var athing = (Essence) Activator.CreateInstance(type, new object[] {null});
					var am = athing.AllowedMaterialsType;

					if (am == EMaterialType.BODY)
					{
						yield return athing;
						continue;
					}

					foreach (var material in m_materials.Where(_material => am.HasFlag(_material.MaterialType)))
					{
						yield return (Essence) Activator.CreateInstance(type, material);
					}
				}
			}
		}

		#endregion
	}
}