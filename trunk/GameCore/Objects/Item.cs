namespace GameCore.Objects
{
	/// <summary>
	/// 	То, что существо может взять и положить в инвентарь
	/// </summary>
	public abstract class Item : Thing
	{
		public override float Opaque
		{
			get { return 0.1f; }
		}
	}
}