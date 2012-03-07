namespace GameCore
{
	public abstract class Material
	{
		protected Material(string _name)
		{
			Name = _name;
		}

		public abstract FColor LerpColor { get; }
		public string Name { get; private set; }
		public abstract EMaterial MaterialType{ get; }
	}

	public enum EMaterial
	{
		METAL,
		WOOD,
		MINERAL,
		FLASH
	}
}
