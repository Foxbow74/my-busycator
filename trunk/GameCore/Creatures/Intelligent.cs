using System.Collections.Generic;
using System.Linq;
using GameCore.Misc;
using GameCore.Objects;

namespace GameCore.Creatures
{
	public abstract class Intelligent : Creature
	{
		readonly BackPack m_backPack = new BackPack();

		protected Intelligent(Point _coords, int _speed) : base(_coords, _speed)
		{
		}

		public void ObjectTaken(Item _item)
		{
		}

		public override IEnumerable<ThingDescriptor> GetBackPackItems()
		{
			return m_backPack.GetItems(this).Items.Select(_item => new ThingDescriptor(_item, Coords, m_backPack));
		}
	}
}