#region

using System;

#endregion

namespace RGL1
{
#if WINDOWS
	internal static class Program
	{
		/// <summary>
		/// 	The main entry point for the application.
		/// </summary>
		private static void Main(string[] _args)
		{
			try
			{
				using (var game = new TheGame())
				{
					game.Run();
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}
	}
#endif
}