namespace GameCore.Objects
{
	public abstract class FurnitureThing : Thing
	{
		public override EThingCategory Category
		{
			get { return EThingCategory.FURNITURE; }
		}
	}
}