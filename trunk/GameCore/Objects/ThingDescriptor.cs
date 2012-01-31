using GameCore.Misc;

namespace GameCore.Objects
{
	public class ThingDescriptor
	{
		private static readonly ThingDescriptor m_empty = new ThingDescriptor(null,null,null);

		/// <summary>
		/// Где лежит (если null - на земле)
		/// </summary>
		public Container Container { get; private set; }

		public Point WorldCoords { get; private set; }

		public Thing Thing { get; private set; }

		public ThingDescriptor(Thing _thing, Point _worldCoords, Container _container)
		{
			Thing = _thing;
			WorldCoords = _worldCoords;
			Container = _container;
		}

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
	}
}
