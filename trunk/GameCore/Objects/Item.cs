namespace GameCore.Objects
{
	public abstract class Item : Thing
	{
		public int UiOrderIndex
		{
			get { return 1; }
		}

		public override int GetHashCode()
		{
			return CalcHashCode();
		}

		protected int CalcHashCode()
		{
			return GetType().GetHashCode();
		}
	}
}