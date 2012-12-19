using System;
using GameCore.Creatures;
using GameCore.Essences.Faked;
using GameCore.Essences.Things;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Essences
{
	public class EssenceDescriptor
	{
		private readonly Creature m_creature;
		private static readonly EssenceDescriptor m_empty = new EssenceDescriptor(null, null, null, null);

		public EssenceDescriptor(Essence _essence, LiveMapCell _cell, IContainer _container, Creature _creature)
		{
			m_creature = _creature;
			if (_essence is IFaked)
			{
				if (_container != null)
				{
					if (_essence is Item)
					{
						_essence = _container.ResolveFakeItem(_creature, (FakedItem)_essence);
					}
					else
					{
						throw new NotImplementedException("Как так?");
					}
				}
				else if(_cell!=null)
				{
					if (_essence is Item)
					{
						_essence = _cell.ResolveFakeItem(_creature, (FakedItem)_essence);
					}
					else if (_essence is Thing)
					{
						_essence = _cell.ResolveFakeThing(_creature);
					}
					
				}
				else
				{
					throw new NotImplementedException("Как так?");
				}
				
			}
			Essence = _essence;
			LiveCoords = _cell!=null?_cell.LiveCoords:null;
			Container = _container;
		}

		/// <summary>
		/// 	Где лежит (если null - на земле)
		/// </summary>
		public IContainer Container { get; private set; }

		public Point LiveCoords { get; private set; }

		public Essence Essence { get; private set; }

		public string UiOrderIndex { get { return Essence.GetName(World.TheWorld.Avatar).Text; } }

		public static EssenceDescriptor Empty { get { return m_empty; } }

		public override int GetHashCode() { return Essence.GetHashCode(); }
	}
}