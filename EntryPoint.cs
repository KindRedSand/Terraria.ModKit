using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

using Razorwing.Framework.IO.Stores;
using Razorwing.Framework.Logging;
using Razorwing.Framework.Platform;

using ReLogic.OS;

using Terraria.ModKit.Razorwing.Overrides;

namespace Terraria.ModKit
{
    internal class EntryPoint
    {
        private static void Main(string[] args)
        {
            try
            {
                Logger.GameIdentifier = "Terraria";
                Logger.VersionIdentifier = "1.4.4";
                Logger.Log("Loading store...");
                Entry.Storage = new ModStorage(@"Creative");
                Entry.Store = new ResourceStore<byte[]>(new StorageBackedResourceStore(Entry.Storage));

                Logger.Storage = Entry.Storage;
                Logger.Level = LogLevel.Debug;

                Logger.Log("Start pre loading assemblies..");
                AppDomain.CurrentDomain.AssemblyResolve += loaderdelegate;
                Logger.Log("Finished assemblies pre loading loading");


                //Force load Newtonsoft.Json to memory
                Logger.Log("Force loading Newtonsoft.Json...");
                string text = Array.Find<string>(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith("Newtonsoft.Json.dll"));
                Assembly result;
                using (Stream manifestResourceStream = typeof(Program).Assembly.GetManifestResourceStream(text))
                {
                    byte[] array = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(array, 0, array.Length);
                    result = AppDomain.CurrentDomain.Load(array);
                    //result = Assembly.Load(array); 
                }
                //Newtonsoft.Json.JsonConvert.SerializeObject()
                var s = (string)Reflect.InvokeS(result, "Newtonsoft.Json.JsonConvert", "SerializeObject", new object[] { new List<string>{"Terraria.ModKit", "v0.7.1"}});
                Console.WriteLine(s);
                Logger.Log("Done! Registering initializer...");
                //Program.LaunchGame(args);

                Program.LaunchParameters = Utils.ParseArguements(args);
                Program.SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Terraria");

                Terraria.Main.OnEngineLoad += Entry.Initialise;


                Logger.Log("Running game...");
                Reflect.Invoke(typeof(WindowsLaunch), "Main", BindingFlags.Static | BindingFlags.NonPublic,
                    args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        private static Assembly loaderdelegate(object sender, ResolveEventArgs sargs)
        {
            bool mono = false;
            string resourceName = new AssemblyName(sargs.Name).Name + ".dll";
            string textx = Array.Find<string>(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
            if (textx == null)
            {
                textx = Array.Find<string>(typeof(EntryPoint).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
                if (textx == null)
                {
                    return null;
                }

                mono = true;
            }
            Assembly resultt;
            if (!mono)
            {
                using (Stream manifestResourceStream = typeof(Program).Assembly.GetManifestResourceStream(textx))
                {
                    byte[] array = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(array, 0, array.Length);
                    resultt = Assembly.Load(array);
                }
            }
            else
            {
                using (Stream manifestResourceStream = typeof(EntryPoint).Assembly.GetManifestResourceStream(textx))
                {
                    byte[] array = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(array, 0, array.Length);
                    resultt = Assembly.Load(array);
                }
            }
            Logger.Log($"Loaded {resultt.FullName}");
            return resultt;
        }
    }
}