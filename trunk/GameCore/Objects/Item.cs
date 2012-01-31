namespace GameCore.Objects
{
	/// <summary>
	/// То, что существо может взять и положить в инвентарь
	/// </summary>
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