using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Misc;

namespace GameCore.Objects.Tools
{
	abstract class LightSourceItem : Item, ILightSource
	{
		private LightSource m_lightSource;

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			m_lightSource.LightCells(_liveMap, _point);
		}

		public override void Resolve(Creature _creature)
		{
			m_lightSource = GetLightSource();
		}

		protected abstract LightSource GetLightSource();
	}

	class Torch : LightSourceItem
	{
		public override ETiles Tile
		{
			get { return ETiles.TORCH; }
		}

		public override string Name
		{
			get { return "факел"; }
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.TOOLS; }
		}

		protected override LightSource GetLightSource()
		{
			return new LightSource(4, new FColor(1f, 1f, 0.9f, 0.5f));
		}
	}
}
