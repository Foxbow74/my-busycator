namespace GameCore.Creatures
{
	/// <summary>
	/// Дает приблизительную оценку размера существа, исходя из которой можно судить о,
	/// эффективности того или иного метода борьбы с ним.
	/// Например кувалда или меч бессильны против мухи
	/// </summary>
	public enum ECreatureSize
	{
		FLY_SIZE,
		VERMIN_SIZE,
		DOG_SIZE,
		HUMAN_SIZE,
		BEAR_SIZE,
		ELEPHANT_SIZE,
		DRAGON_SIZE,
	}
}