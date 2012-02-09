namespace GameCore.Objects.Furniture
{
	public abstract class Furniture : Thing
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE; }
		}
	}
}