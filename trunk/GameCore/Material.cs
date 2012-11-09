using GameCore.Creatures;
using RusLanguage;

namespace GameCore
{
	public abstract class Material
	{
		protected Material(string _name)
		{
			Name = _name;
			Sex = ESex.MALE;
		}

		public ESex Sex { get; protected set; }

		public abstract FColor LerpColor { get; }
		public string Name { get; private set; }
		public abstract EMaterial MaterialType { get; }

		/// <summary>
		/// Насколько часто встречается, максимум - 100
		/// чем дальше от стартовой точки, тем чаще встречаются редкие и реже частые
		/// </summary>
		public virtual float Frequency { get { return 100; } }

		public string this[EPadej _padej] { get { return this[_padej, World.TheWorld.Avatar]; } }

		public string this[EPadej _padej, Creature _creature] { get { return Sklonenia.ToPadej(_padej, Name, false, Sex); } }

		public override string ToString()
		{
			return "[" + Name + "]";
		}
	}
}