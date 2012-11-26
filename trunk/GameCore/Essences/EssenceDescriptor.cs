using GameCore.Creatures;
using GameCore.Essences.Faked;
using GameCore.Essences.Things;
using GameCore.Misc;

namespace GameCore.Essences
{
	public class EssenceDescriptor
	{
		private static readonly EssenceDescriptor m_empty = new EssenceDescriptor(null, null, null);

		public EssenceDescriptor(Essence _essence, Point _liveCoords, IContainer _container)
		{
			Essence = _essence;
			LiveCoords = _liveCoords;
			Container = _container;
		}

		/// <summary>
		/// 	Где лежит (если null - на земле)
		/// </summary>
		public IContainer Container { get; private set; }

		public Point LiveCoords { get; private set; }

		public Essence Essence { get; private set; }

		public string UiOrderIndex { get { return Essence.GetName(World.TheWorld.Avatar); } }

		public static EssenceDescriptor Empty { get { return m_empty; } }

		public override int GetHashCode() { return Essence.GetHashCode(); }

		public Essence ResolveEssence(Creature _creature)
		{
			if (Essence is IFaked)
			{
				var liveMapCell = World.TheWorld.LiveMap.GetCell(LiveCoords);
				if (Essence is Item)
				{
					Essence = liveMapCell.ResolveFakeItem(_creature, (FakedItem) Essence);
				}
				else if (Essence is Thing)
				{
					Essence = liveMapCell.ResolveFakeThing(_creature);
				}
			}
			return Essence;
		}
	}
}