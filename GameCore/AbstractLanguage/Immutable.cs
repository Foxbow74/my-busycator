namespace GameCore.AbstractLanguage
{
	/// <summary>
	/// неизменное при склонении слово или сочетание
	/// </summary>
	public class Immutable : AbstractWord
	{
		public Immutable(string _text)
			: base(_text)
		{
		}
	}
}