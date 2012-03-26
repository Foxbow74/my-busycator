using System.Collections.Generic;

namespace GameCore.PathFinding
{
	public class PriorityQueueB<T>
	{
		#region Variables Declaration

		private readonly IComparer<T> m_comparer;
		protected List<T> InnerList = new List<T>();

		#endregion

		#region Contructors

		public PriorityQueueB() { m_comparer = Comparer<T>.Default; }

		public PriorityQueueB(IComparer<T> _comparer) { m_comparer = _comparer; }

		public PriorityQueueB(IComparer<T> _comparer, int _capacity)
		{
			m_comparer = _comparer;
			InnerList.Capacity = _capacity;
		}

		#endregion

		#region Methods

		public int Count { get { return InnerList.Count; } }

		public T this[int _index]
		{
			get { return InnerList[_index]; }
			set
			{
				InnerList[_index] = value;
				Update(_index);
			}
		}

		protected void SwitchElements(int _i, int _j)
		{
			var h = InnerList[_i];
			InnerList[_i] = InnerList[_j];
			InnerList[_j] = h;
		}

		protected virtual int OnCompare(int _i, int _j) { return m_comparer.Compare(InnerList[_i], InnerList[_j]); }

		/// <summary>
		/// 	Push an object onto the PQ
		/// </summary>
		/// <param name = "O">The new object</param>
		/// <param name = "_item"></param>
		/// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
		public int Push(T _item)
		{
			var p = InnerList.Count;
			InnerList.Add(_item); // E[p] = O
			do
			{
				if (p == 0)
					break;
				var p2 = (p - 1)/2;
				if (OnCompare(p, p2) < 0)
				{
					SwitchElements(p, p2);
					p = p2;
				}
				else
					break;
			} while (true);
			return p;
		}

		/// <summary>
		/// 	Get the smallest object and remove it.
		/// </summary>
		/// <returns>The smallest object</returns>
		public T Pop()
		{
			var result = InnerList[0];
			var p = 0;
			InnerList[0] = InnerList[InnerList.Count - 1];
			InnerList.RemoveAt(InnerList.Count - 1);
			do
			{
				var pn = p;
				var p1 = 2*p + 1;
				var p2 = 2*p + 2;
				if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
					p = p1;
				if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
					p = p2;

				if (p == pn)
					break;
				SwitchElements(p, pn);
			} while (true);

			return result;
		}

		/// <summary>
		/// 	Notify the PQ that the object at position i has changed
		/// 	and the PQ needs to restore order.
		/// 	Since you dont have access to any indexes (except by using the
		/// 	explicit IList.this) you should not call this function without knowing exactly
		/// 	what you do.
		/// </summary>
		/// <param name = "_i">The index of the changed object.</param>
		public void Update(int _i)
		{
			var p = _i;
			int p2;
			do // aufsteigen
			{
				if (p == 0)
					break;
				p2 = (p - 1)/2;
				if (OnCompare(p, p2) < 0)
				{
					SwitchElements(p, p2);
					p = p2;
				}
				else
					break;
			} while (true);
			if (p < _i)
				return;
			do // absteigen
			{
				var pn = p;
				var p1 = 2*p + 1;
				p2 = 2*p + 2;
				if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
					p = p1;
				if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
					p = p2;

				if (p == pn)
					break;
				SwitchElements(p, pn);
			} while (true);
		}

		/// <summary>
		/// 	Get the smallest object without removing it.
		/// </summary>
		/// <returns>The smallest object</returns>
		public T Peek()
		{
			if (InnerList.Count > 0)
				return InnerList[0];
			return default(T);
		}

		public void Clear() { InnerList.Clear(); }

		public void RemoveLocation(T _item)
		{
			var index = -1;
			for (var i = 0; i < InnerList.Count; i++)
			{
				if (m_comparer.Compare(InnerList[i], _item) == 0)
					index = i;
			}

			if (index != -1)
				InnerList.RemoveAt(index);
		}

		#endregion
	}
}