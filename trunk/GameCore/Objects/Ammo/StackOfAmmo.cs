using System;
using GameCore.Creatures;

namespace GameCore.Objects.Ammo
{
	abstract class StackOfAmmo:Item
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.MISSILES; }
		}

		public int Count { get; set; }

		public override void Resolve(Creature _creature)
		{
			Count = (int)(_creature.GetLuckRandom * 25) + 1;
		}

		public override string Name
		{
			get
			{
				if(Count==0)
				{
					throw new ApplicationException("Стек пуст");
				}
				return InternalName + " * " + Count;
			}
		}

		protected abstract string InternalName { get; }
	}
}
