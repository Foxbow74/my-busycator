using GameCore.AbstractLanguage;

namespace GameCore
{
	public abstract class Material
	{
		protected Material(string _name)
		{
			Name = _name.AsAdj();
			Sex = ESex.MALE;
		}

		public ESex Sex { get; protected set; }

		public abstract FColor LerpColor { get; }
		public Adjective Name { get; private set; }
		public abstract EMaterialType MaterialType { get; }

		/// <summary>
		/// Насколько часто встречается, максимум - 100
		/// чем дальше от стартовой точки, тем чаще встречаются редкие и реже частые
		/// </summary>
		public virtual float Frequency { get { return 100; } }

		public override string ToString()
		{
			return "[" + Name + "]";
		}
	}
}