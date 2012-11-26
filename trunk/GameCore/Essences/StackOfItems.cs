using System;
using GameCore.Creatures;
using GameCore.Essences.Ammo;

namespace GameCore.Essences
{
	public abstract class StackOfItems : Item
	{
		protected StackOfItems(Material _material) : base(_material) { }

		public int Count { get; set; }

		public override string Name
		{
			get
			{
				if (Count == 0)
				{
					throw new ApplicationException("Стек пуст");
				}
				if (Count == 1)
				{
					return NameOfSingle;
				}
				return NameOfSingle + " * " + Count;
			}
		}

		protected abstract string NameOfSingle { get; }
		protected abstract int GetStartCount(Creature _creature);

		public override void Resolve(Creature _creature) { if (Count == 0) Count = GetStartCount(_creature); }

		public StackOfItems GetOne()
		{
			Count--;
			var stackOfAmmo = (StackOfAmmo) Activator.CreateInstance(GetType(), Material);
			stackOfAmmo.Count = 1;
			return stackOfAmmo;
		}

		public void Add(StackOfItems _stackOfItems)
		{
			if (!Equals(_stackOfItems))
			{
				throw new ApplicationException("не одинаковые");
			}
			Count += _stackOfItems.Count;
			_stackOfItems.Count = 0;
		}
	}
}