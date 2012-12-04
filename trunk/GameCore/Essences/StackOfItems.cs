using System;
using System.Globalization;
using GameCore.AbstractLanguage;
using GameCore.Creatures;

namespace GameCore.Essences
{
	public abstract class StackOfItems : Item
	{
		private readonly Noun m_nameOfItem;

		protected StackOfItems(Noun _nameOfItem, Material _material) : base("кучка".AsNoun(ESex.FEMALE, false) + "чего-то".AsOf(), _material)
		{
			m_nameOfItem = _nameOfItem;
		}

		public int Count { get; set; }

		public override Noun Name
		{
			get
			{
				if (Count == 0)
				{
					return base.Name;
				}
				if (Count == 1)
				{
					return m_nameOfItem;
				}
				return m_nameOfItem + (" * " + Count.ToString(CultureInfo.InvariantCulture)).AsIm();
			}
		}

		protected abstract int GetStartCount(Creature _creature);

		internal override Essence Clone(Creature _resolver)
		{
			var clone = (StackOfItems)base.Clone(_resolver);
			if (clone.Count == 0)
			{
				clone.Count = clone.GetStartCount(_resolver);
			}
			return clone;
		}

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