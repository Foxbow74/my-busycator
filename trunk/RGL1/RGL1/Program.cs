namespace RGL1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] _args)
        {
            using (var game = new TheGame())
            {
                game.Run();
            }
        }
    }
#endif
}

