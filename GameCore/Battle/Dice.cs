using System.Globalization;

namespace GameCore.Battle
{
	/// <summary>
	/// 2d8+1
	/// </summary>
	public class Dice
	{
		public Dice(int _count, int _size, int _modifier)
		{
			Count = _count;
			Size = _size;
			Modifier = _modifier;
		}

		public Dice(int _count, int _size)
		{
			Count = _count;
			Size = _size;
			Modifier = 0;
		}

		public int Count { get; set; }
		public int Size { get; set; }
		public int Modifier { get; set; }
		public int Max { get { return Count*Size + Modifier; } }

		public override string ToString()
		{
			return string.Format("{0}d{1}{2}", Count, Size,(Modifier == 0? "": (Modifier > 0 ? ("+" + Modifier) : Modifier.ToString(CultureInfo.InvariantCulture))));
		}

		public int Roll()
		{
			var result = Modifier;
			for (var i = 0; i < Count; i++)
			{
				result += World.Rnd.Next(Size+1);
			}
			return result;
		}
	}
}