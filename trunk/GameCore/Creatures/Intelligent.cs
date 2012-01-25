#region

using GameCore.Misc;
using GameCore.Objects;

#endregion

namespace GameCore.Creatures
{
	public abstract class Intelligent : Creature
	{
		protected Intelligent(Point _coords, int _speed) : base(_coords, _speed)
		{
			Inventory = new Inventory();
		}

		public Inventory Inventory { get; private set; }

		public void ObjectTaken(Item _item)
		{
			Inventory.Add(_item);
		}
	}
}