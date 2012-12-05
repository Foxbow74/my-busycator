using System;
using System.Globalization;
using GameCore.AbstractLanguage;
using GameCore.Creatures;

namespace GameCore.Essences
{
	public abstract class StackOfItems : Item
	{
		private readonly EALNouns m_nameOfItem;

		protected StackOfItems(EALNouns _nameOfItem, Material _material)
			: base(EALNouns.StackOf, _material)
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
					return m_nameOfItem.AsNoun();
				}
				return m_nameOfItem.AsNoun() + (" * " + Count.ToString(CultureInfo.InvariantCulture)).AsIm();
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