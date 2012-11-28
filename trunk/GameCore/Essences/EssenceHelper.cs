using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Creatures;
using GameCore.Essences.Faked;
using GameCore.Essences.Things;
using GameCore.Mapping;
using GameCore.Materials;
using GameCore.Misc;
using GameCore.Storage;

namespace GameCore.Essences
{
	public static class EssenceHelper
	{
		#region Fields

		private static readonly Dictionary<Tuple<ETileset, Material, int>, FakedCreature> m_fakedCreatures = new Dictionary<Tuple<ETileset, Material, int>, FakedCreature>();
		private static readonly Dictionary<Tuple<ETileset, Material, int>, FakedItem> m_fakedItems = new Dictionary<Tuple<ETileset, Material, int>, FakedItem>();
		private static readonly Dictionary<Tuple<ETileset, Material, int>, FakedThing> m_fakedThings = new Dictionary<Tuple<ETileset, Material, int>, FakedThing>();
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

				if (typeof (Creature).IsAssignableFrom(type))
				{
					RegisterCreatureType(type);
				}
				else if (typeof (Item).IsAssignableFrom(type))
				{
					RegisterItemType(type);
				}
				else
				{
					RegisterThingType(type);
				}
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

		private static IEnumerable<EMaterial> GetAllowedMaterials(EMaterial _materials)
		{
			var allowedMaterials = (from EMaterial value in Enum.GetValues(typeof (EMaterial)) where _materials.HasFlag(value) select value).ToArray();
			return allowedMaterials;
		}

		public static FakedCreature GetFakedCreature(MapBlock _block)
		{
			var keys = new List<Tuple<ETileset, Material, int>>(m_fakedCreatures.Keys);
			return m_fakedCreatures[keys[World.Rnd.Next(keys.Count)]];
		}

		public static FakedItem GetRandomFakedItem(Random _rnd)
		{
			return m_fakedItems.Values.ToArray().RandomItem(_rnd);
		}

		public static FakedItem GetRandomFakedItem<T>(Random _rnd) where T : Item
		{
			return m_fakedItems.Values.Where(_item=>_item.Is<T>()).ToArray().RandomItem(_rnd);
		}

		public static FakedCreature GetRandomFakedCreature<T>(Random _rnd) where T:Creature
		{
			return m_fakedCreatures.Values.Where(_creature => _creature.Is<T>()).ToArray().RandomItem(_rnd);
		}

		public static Essence GetFakedThing(Random _rnd)
		{
			return m_fakedThings.Values.ToArray().RandomItem(_rnd);
		}

		public static FakedThing GetFirstFoundedThing<T>() where T : Thing
		{
			return m_fakedThings.Values.First(_thing => _thing.Is<T>());
		}

		public static FakedItem GetFirstFoundedItem<T>() where T : Item
		{
			return m_fakedItems.Values.First(_item => _item.Is<T>());
		}
		
		public static FakedCreature GetFirstFoundedCreature<T>() where T : Creature
		{
			return m_fakedCreatures.Values.First(_item => _item.Is<T>());
		}

		public static Material GetFirstFoundedMaterial<T>() where T : Material
		{
			return m_materials.First(_item => _item is T);
		}

		public static IEnumerable<FakedThing> GetAllThings<T>() where T : Thing
		{
			return m_fakedThings.Values.Where(_thing => _thing.Is<T>());
		}

		public static IEnumerable<FakedItem> GetAllItems<T>() where T : Item
		{
			return m_fakedItems.Values.Where(_item => _item.Is<T>());
		}

		public static IEnumerable<FakedCreature> GetAllCreatures<T>() where T : Item
		{
			return m_fakedCreatures.Values.Where(_item => _item.Is<T>());
		}

		public static TMaterial GetMaterial<TMaterial>() where TMaterial : Material
		{
			return (TMaterial) m_materials.First(_material => _material is TMaterial);
		}

		/// <summary>
		/// 	���������� ��� ������� ���� ���� - ��������
		/// </summary>
		/// <param name="_essence"> </param>
		/// <param name="_creature"> </param>
		/// <param name="_cell"> ���������� �� ������ ��������� � ������������ �������� </param>
		/// <returns> </returns>
		public static string GetName(this Essence _essence, Creature _creature, LiveMapCell _cell = null)
		{
			if (_cell == null)
			{
				_cell = _creature[Point.Zero];
			}
			if (_essence is IFaked)
			{
				var liveMapCell = _cell;
				if (_essence is FakedItem)
				{
					_essence = liveMapCell.ResolveFakeItem(World.TheWorld.Avatar, (FakedItem) _essence);
				}
				else if (_essence is FakedThing)
				{
					_essence = liveMapCell.ResolveFakeThing(_creature);
				}
			}
			if(_essence is Creature)
			{
				if(_essence is Intelligent)
				{
					return _essence.Name;
				}
				return _essence.Name;
			}
			return _essence.Name;
		}

		public static string GetName(this EssenceDescriptor _essenceDescriptor, Creature _creature)
		{
			var thing = _essenceDescriptor.Essence;
			if (thing is IFaked)
			{
				thing = _essenceDescriptor.ResolveEssence(_creature);
			}
			return thing.Name;
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

		public static Essence ResolveEssence(Type _type, Material _material, Creature _creature)
		{
			var thing = (Essence) Activator.CreateInstance(_type, _material);
			thing.Resolve(_creature);
			return thing;
		}

		#region Register

		private static void RegisterCreatureType(Type _type)
		{
			var thing = (Essence) Activator.CreateInstance(_type, new object[] {null});
			FakedCreature value;
			var key = new Tuple<ETileset, Material, int>(thing.Tileset, GetMaterial<BodyMaterial>(), thing.TileIndex);
			if (!m_fakedCreatures.TryGetValue(key, out value))
			{
				value = new FakedCreature(thing.Tileset);
				m_fakedCreatures.Add(key, value);
			}
			value.Add(_type);
		}

		private static void RegisterItemType(Type _type)
		{
			var athing = (Essence) Activator.CreateInstance(_type, new object[] {null});
			foreach (var mtp in GetAllowedMaterials(athing.AllowedMaterials))
			{
				var mtpLocal = mtp;
				foreach (var material in m_materials)
				{
					if(material.MaterialType != mtpLocal)
					{
						continue;
					}

					var thing = (Essence) Activator.CreateInstance(_type, material);
					FakedItem value;
					var key = new Tuple<ETileset, Material, int>(thing.Tileset, material, thing.TileIndex);
					if (!m_fakedItems.TryGetValue(key, out value))
					{
						value = new FakedItem(thing.Tileset, material, thing.TileIndex);
						m_fakedItems.Add(key, value);
					}
					value.Add(_type);
				}
			}
		}

		private static void RegisterThingType(Type _type)
		{
			var athing = (Essence) Activator.CreateInstance(_type, new object[] {null});
			foreach (var mtp in GetAllowedMaterials(athing.AllowedMaterials))
			{
				var mtpLocal = mtp;
				foreach (var material in m_materials)
				{
					if (material.MaterialType != mtpLocal)
					{
						continue;
					}

					var thing = (Essence) Activator.CreateInstance(_type, material);
					var key = new Tuple<ETileset, Material, int>(thing.Tileset, material, thing.TileIndex);
					FakedThing value;
					if (!m_fakedThings.TryGetValue(key, out value))
					{
						value = new FakedThing(thing);
						m_fakedThings.Add(key, value);
					}
				}
			}
		}

		#endregion

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
					var am = athing.AllowedMaterials;

					if (am == EMaterial.BODY)
					{
						yield return athing;
						continue;
					}

					foreach (var material in m_materials.Where(_material => am.HasFlag(_material.MaterialType)))
					{
						if (am.HasFlag(material.MaterialType))
						{
							continue;
						}
						yield return (Essence) Activator.CreateInstance(type, material);
					}
				}
			}
		}

		#endregion
	}
}