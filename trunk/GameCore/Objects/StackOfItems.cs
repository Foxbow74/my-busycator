﻿using System;
using GameCore.Creatures;
using GameCore.Objects.Ammo;

namespace GameCore.Objects
{
	internal abstract class StackOfItems : Item
	{
		public int Count { get; private set; }

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

		public override void Resolve(Creature _creature)
		{
			Count = GetStartCount(_creature);
		}

		public StackOfItems GetOne()
		{
			Count--;
			var stackOfAmmo = (StackOfAmmo) Activator.CreateInstance(GetType());
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