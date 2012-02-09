using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Objects
{
	public class ThingDescriptor
	{
		private static readonly ThingDescriptor m_empty = new ThingDescriptor(null, null, null);

		public ThingDescriptor(Thing _thing, Point _worldCoords, IContainer _container)
		{
			Thing = _thing;
			WorldCoords = _worldCoords;
			Container = _container;
		}

		/// <summary>
		/// 	Где лежит (если null - на земле)
		/// </summary>
		public IContainer Container { get; private set; }

		public Point WorldCoords { get; private set; }

		public Thing Thing { get; private set; }

		public string UiOrderIndex
		{
			get { return Thing.Name; }
		}

		public static ThingDescriptor Empty
		{
			get { return m_empty; }
		}

		public override int GetHashCode()
		{
			return Thing.GetHashCode();
		}

		public Thing ResolveThing(Creature _creature)
		{
			if (Thing is IFaked)
			{
				var mapCell = _creature.Layer.GetMapCell(WorldCoords);
				if (Thing is Item)
				{
					Thing = mapCell.ResolveFakeItem(_creature, (FakedItem) Thing);
				}
				else if (Thing is Furniture.Furniture)
				{
					Thing = mapCell.ResolveFakeFurniture(_creature, (FakedThing) Thing);
				}
			}
			return Thing;
		}
	}
}