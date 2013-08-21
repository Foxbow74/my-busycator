namespace GameCore.AbstractLanguage
{
	/// <summary>
	/// XX чего либо, например, король "воров"
	/// </summary>
	public class OfSomething : Noun
	{
		public OfSomething(Noun noun)
			: base(noun.Text, noun.Sex, noun.IsCreature)
		{
			Adjective = noun.Adjective;
			CoName = noun.CoName;
			AlsoKnownAs = noun.AlsoKnownAs;
		}
	}
}