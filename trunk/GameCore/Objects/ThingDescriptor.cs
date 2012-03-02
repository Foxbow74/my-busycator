using GameCore.Creatures;
using GameCore.Misc;
using GameCore.Objects.Furniture;

namespace GameCore.Objects
{
	public class ThingDescriptor
	{
		private static readonly ThingDescriptor m_empty = new ThingDescriptor(null, null, null);

		public ThingDescriptor(Thing _thing, Point _liveCoords, IContainer _container)
		{
			Thing = _thing;
			LiveCoords = _liveCoords;
			Container = _container;
		}

		/// <summary>
		/// 	Где лежит (если null - на земле)
		/// </summary>
		public IContainer Container { get; private set; }

		public Point LiveCoords { get; private set; }

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
				var liveMapCell = World.TheWorld.LiveMap.GetCell(LiveCoords);
				if (Thing is Item)
				{
					Thing = liveMapCell.ResolveFakeItem(_creature, (FakedItem) Thing);
				}
				else if (Thing is FurnitureThing)
				{
					Thing = liveMapCell.ResolveFakeFurniture(_creature);
				}
			}
			return Thing;
		}
	}
}