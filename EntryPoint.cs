using System;
using System.Reflection;

namespace Terraria.ModKit
{
    internal class EntryPoint
    {
        private static void Main(string[] args)
        {
            try
            {
                //Program.LaunchGame(args);
                Terraria.Main.OnEngineLoad += Entry.Initialise;
                Reflect.Invoke(typeof(WindowsLaunch), "Main", BindingFlags.Static | BindingFlags.NonPublic,
                    args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}