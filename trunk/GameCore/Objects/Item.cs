namespace GameCore.Objects
{
	public abstract class Item : Thing
	{
		public override int GetHashCode()
		{
			return CalcHashCode();
		}

		protected int CalcHashCode()
		{
			return GetType().GetHashCode();
		}

		public override float Opaque
		{
			get
			{
				return 0;
			}
		}
	}
}